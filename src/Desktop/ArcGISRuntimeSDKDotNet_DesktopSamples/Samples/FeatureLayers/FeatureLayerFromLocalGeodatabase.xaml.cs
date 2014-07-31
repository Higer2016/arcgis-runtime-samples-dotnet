﻿using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ArcGISRuntimeSDKDotNet_DesktopSamples.Samples
{
    /// <summary>
    /// This sample shows how to add a FeatureLayer from a local .geodatabase file to the map. A .geodatabase file may be created using the 'Create Runtime Content' tool in ArcMap.
    /// </summary>
    /// <title>Feature Layer from Local Geodatabase</title>
	/// <category>Layers</category>
	/// <subcategory>Feature Layers</subcategory>
	public partial class FeatureLayerFromLocalGeodatabase : UserControl
    {
        private const string GDB_PATH = @"..\..\..\..\..\samples-data\maps\usa.geodatabase";

        /// <summary>Construct FeatureLayerFromLocalGeodatabase sample control</summary>
        public FeatureLayerFromLocalGeodatabase()
        {
            InitializeComponent();
            var _ = CreateFeatureLayersAsync();
        }

        private async Task CreateFeatureLayersAsync()
        {
            try
            {
                var gdb = await Geodatabase.OpenAsync(GDB_PATH);

				Envelope extent = null;
                foreach (var table in gdb.FeatureTables)
                {
                    var flayer = new FeatureLayer()
                    {
                        ID = table.Name,
                        DisplayName = table.Name,
                        FeatureTable = table
                    };

					if (table.ServiceInfo.Extent != null &&
						!table.ServiceInfo.Extent.IsEmpty)
					{
						if (extent.IsEmpty)
							extent = table.ServiceInfo.Extent;
						else
							extent = extent.Union(table.ServiceInfo.Extent);
					}

					MyMapView.Map.Layers.Add(flayer);
                }

				await MyMapView.SetViewAsync(extent.Expand(1.10));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating feature layer: " + ex.Message, "Samples");
            }
        }
    }
}
