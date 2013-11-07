﻿using System;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Dev2.Activities.Designers2.DataMerge;
using Dev2.Studio.Core.Activities.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unlimited.Applications.BusinessDesignStudio.Activities;

namespace Dev2.Activities.Designers.Tests.DataMerge
{
    [TestClass][ExcludeFromCodeCoverage]
    public class DataMergeTests
    {
        [TestMethod]
        [Owner("Tshepo Ntlhokoa")]
        [TestCategory("DataMergeViewModel_Constructor")]
        public void DataMergeViewModel_Constructor__ModelItemIsValid_ListHasFourItems()
        {
            var items = new List<DataMergeDTO> { new DataMergeDTO("", "None", "", 0, "", "Left", false) };
            var viewModel = new DataMergeDesignerViewModel(CreateModelItem(items));
            Assert.AreEqual(5, viewModel.ItemsList.Count);
        }
        
        [TestMethod]
        [Owner("Tshepo Ntlhokoa")]
        [TestCategory("DataMergeViewModel_Constructor")]
        public void DataMergeViewModel_Constructor__ModelItemIsValid_CollectionNameIsSetToMergeCollection()
        {
            var items = new List<DataMergeDTO> { new DataMergeDTO() };
            var viewModel = new DataMergeDesignerViewModel(CreateModelItem(items));
            Assert.AreEqual("MergeCollection", viewModel.CollectionName);
        }

        [TestMethod]
        [Owner("Tshepo Ntlhokoa")]
        [TestCategory("DataMergeViewModel_Constructor")]
        public void DataMergeViewModel_Constructor_ModelItemIsValid_MergeCollectionHasTwoItems()
        {
            var modelItem = ModelItemUtils.CreateModelItem(new DsfDataMergeActivity());
            var viewModel = new DataMergeDesignerViewModel(modelItem);
            dynamic mi = viewModel.ModelItem;
            Assert.AreEqual(2, mi.MergeCollection.Count);
        }

        [TestMethod]
        [Owner("Tshepo Ntlhokoa")]
        [TestCategory("DataMergeViewModel_Constructor")]
        public void DataMergeViewModel_Constructor_ModelItemIsInitializedWith4Items_MergeCollectionHasFourItems()
        {
            var items = new List<DataMergeDTO>
            {
                new DataMergeDTO("", "None", "", 0, "", "Left"),
                new DataMergeDTO("", "None", "", 0, "", "Left"),
                new DataMergeDTO("", "None", "", 0, "", "Left"),
                new DataMergeDTO("", "None", "", 0, "", "Left")
            };
            var viewModel = new DataMergeDesignerViewModel(CreateModelItem(items));
            dynamic mi = viewModel.ModelItem;
            Assert.AreEqual(4, mi.MergeCollection.Count);
        }

        [TestMethod]
        [Owner("Tshepo Ntlhokoa")]
        [TestCategory("DataMergeViewModel_OnMergeTypeChanged")]
        public void DataMergeViewModel_OnMergeTypeChanged_SetIndexToMergeTypeToNone_EnableAtIsSetToFalse()
        {
            VerifyMergeTypeAgaintsEnabledAt("None", false);
        }

        [TestMethod]
        [Owner("Tshepo Ntlhokoa")]
        [TestCategory("DataMergeViewModel_OnMergeTypeChanged")]
        public void DataMergeViewModel_OnMergeTypeChanged_SetIndexToMergeTypeToTab_EnableAtIsSetToFalse()
        {
            VerifyMergeTypeAgaintsEnabledAt("Tab", false);
        }

        [TestMethod]
        [Owner("Tshepo Ntlhokoa")]
        [TestCategory("DataMergeViewModel_OnMergeTypeChanged")]
        public void DataMergeViewModel_OnMergeTypeChanged_SetIndexToMergeTypeToNewLine_EnableAtIsSetToFalse()
        {
            VerifyMergeTypeAgaintsEnabledAt("New Line", false);
        }

        [TestMethod]
        [Owner("Tshepo Ntlhokoa")]
        [TestCategory("DataMergeViewModel_OnMergeTypeChanged")]
        public void DataMergeViewModel_OnMergeTypeChanged_SetIndexToMergeTypeToIndex_EnableAtIsSetToTrue()
        {
            VerifyMergeTypeAgaintsEnabledAt("Index", true);
        }

        [TestMethod]
        [Owner("Tshepo Ntlhokoa")]
        [TestCategory("DataMergeViewModel_OnMergeTypeChanged")]
        public void DataMergeViewModel_OnMergeTypeChanged_SetIndexToMergeTypeToChars_EnableAtIsSetToTrue()
        {
            VerifyMergeTypeAgaintsEnabledAt("Chars", true);
        }

        static void VerifyMergeTypeAgaintsEnabledAt(string mergeType, bool expectedEnableAt)
        {
            var items = new List<DataMergeDTO> { new DataMergeDTO("", mergeType, "", 0, "", "Left") };
            var viewModel = new DataMergeDesignerViewModel(CreateModelItem(items));
            viewModel.OnMergeTypeChanged(0);
            dynamic mi = viewModel.ModelItemCollection[0];
            var at = mi.At as string;
            var actualEnableAt = mi.EnableAt as bool?;
            Assert.AreEqual("", at);
            Assert.AreEqual(expectedEnableAt, actualEnableAt);
        }

        static ModelItem CreateModelItem(IEnumerable<DataMergeDTO> items, string displayName = "Merge")
        {
            var modelItem = ModelItemUtils.CreateModelItem(new DsfDataMergeActivity());
            modelItem.SetProperty("DisplayName", displayName);

            var modelItemCollection = modelItem.Properties["MergeCollection"].Collection;
            foreach(var dto in items)
            {
                modelItemCollection.Add(dto);
            }
            return modelItem;
        }
    }
}
