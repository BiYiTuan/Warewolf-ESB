using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using Dev2.Common.Interfaces.Core.Graph;
using Dev2.Runtime.ServiceModel.Data;
using Dev2.Services.Sql;
using Unlimited.Framework.Converters.Graph;

namespace Dev2.Runtime.ServiceModel.Esb.Brokers
{
    /// <summary>
    /// A Microsoft SQL specific database broker implementation
    /// </summary>
    public class MySqlDatabaseBroker : AbstractDatabaseBroker<MySqlServer>
    {
        protected override string NormalizeXmlPayload(string payload)
        {
            var result = new StringBuilder();

            var xDoc = new XmlDocument();
            xDoc.LoadXml(payload);
            var nl = xDoc.SelectNodes("//NewDataSet/Table/*[starts-with(local-name(),'XML_')]");
            var foundXMLFrags = 0;

            if(nl != null)
            {
                foreach(XmlNode n in nl)
                {
                    var tmp = n.InnerXml;
                    result = result.Append(tmp);
                    foundXMLFrags++;
                }
            }

            var res = result.ToString();

            if(foundXMLFrags >= 1)
            {
                res = "<FromXMLPayloads>" + res + "</FromXMLPayloads>";
            }
            else if(foundXMLFrags == 0)
            {
                res = payload;
            }

            return base.NormalizeXmlPayload(res);
        }
        public override List<string> GetDatabases(DbSource dbSource)
        {
            VerifyArgument.IsNotNull("dbSource", dbSource);
            using (var server = CreateDbServer(dbSource))
            {
                server.Connect(dbSource.ConnectionString);
                return server.FetchDatabases();
            }
        }

        #region Overrides of AbstractDatabaseBroker<MySqlServer>

        public override ServiceMethodList GetServiceMethods(DbSource dbSource)
        {
            VerifyArgument.IsNotNull("dbSource", dbSource);

            // Check the cache for a value ;)
            ServiceMethodList cacheResult;
            if (!dbSource.ReloadActions)
            {
                if (GetCachedResult(dbSource, out cacheResult))
                {
                    return cacheResult;
                }
            }
            // else reload actions ;)

            var serviceMethods = new ServiceMethodList();

            //
            // Function to handle procedures returned by the data broker
            //
            Func<IDbCommand, IList<IDbDataParameter>, string, string, bool> procedureFunc = (command, parameters, helpText, executeAction) =>
            {
                var serviceMethod = CreateServiceMethod(command, parameters, helpText, executeAction);
                serviceMethods.Add(serviceMethod);
                return true;
            };

            //
            // Function to handle functions returned by the data broker
            //
            Func<IDbCommand, IList<IDbDataParameter>, string, string, bool> functionFunc = (command, parameters, helpText, executeAction) =>
            {
                var serviceMethod = CreateServiceMethod(command, parameters, helpText, executeAction);
                serviceMethods.Add(serviceMethod);
                return true;
            };

            //
            // Get stored procedures and functions for this database source
            //
            using (var server = CreateDbServer(dbSource))
            {
                server.Connect(dbSource.ConnectionString);
                server.FetchStoredProcedures(procedureFunc, functionFunc,false,dbSource.DatabaseName);
            }

            // Add to cache ;)
            TheCache.AddOrUpdate(dbSource.ConnectionString, serviceMethods, (s, list) => serviceMethods);

            return GetCachedResult(dbSource, out cacheResult) ? cacheResult : serviceMethods;
        }

        #region Overrides of AbstractDatabaseBroker<MySqlServer>

        protected override MySqlServer CreateDbServer(DbSource dbSource)
        {
            return new MySqlServer();
        }

        #endregion

        #endregion

        public override IOutputDescription TestService(DbService dbService)
        {
            VerifyArgument.IsNotNull("dbService", dbService);
            VerifyArgument.IsNotNull("dbService.Source", dbService.Source);

            IOutputDescription result;
            using (var server = CreateDbServer(dbService.Source as DbSource))
            {
                server.Connect(((DbSource)dbService.Source).ConnectionString);
                server.BeginTransaction();
                try
                {
                    //
                    // Execute command and normalize XML
                    //
                    var command = CommandFromServiceMethod(server, dbService.Method);
                    var dataTable = server.FetchDataTable(command);

                    //
                    // Map shape of XML
                    //
                    result = OutputDescriptionFactory.CreateOutputDescription(OutputFormats.ShapedXML);
                    var dataSourceShape = DataSourceShapeFactory.CreateDataSourceShape();
                    result.DataSourceShapes.Add(dataSourceShape);

                    var dataBrowser = DataBrowserFactory.CreateDataBrowser();
                    dataSourceShape.Paths.AddRange(dataBrowser.Map(dataTable));
                }
                finally
                {
                    server.RollbackTransaction();
                }
            }

            return result;
        }
    }
}