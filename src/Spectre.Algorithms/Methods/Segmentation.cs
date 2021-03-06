﻿/*
 * SegmentationTests.cs
 * Contains .NET interface for DiviK algorithm.
 *
   Copyright 2017 Wilgierz Wojciech, Michal Gallus, Grzegorz Mrukwa

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using Spectre.Algorithms.Parameterization;
using Spectre.Algorithms.Results;
using Spectre.Data.Datasets;

namespace Spectre.Algorithms.Methods
{
    /// <summary>
    ///     Contains interface for calling matlab Divik algorithm.
    /// </summary>
    public class Segmentation : IDisposable
    {
        #region Fields

        /// <summary>
        ///     Locates divik structure in matlab result array
        /// </summary>
        private const int DivikStructureLocation = 1;

        private readonly MatlabAlgorithmsNative.Segmentation _segmentation;

        /// <summary>
        ///     Indicates whether this instance has been disposed.
        /// </summary>
        private bool _disposed;

        #endregion

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="Segmentation" /> class.
        /// </summary>
        public Segmentation()
        {
            _segmentation = new MatlabAlgorithmsNative.Segmentation();
        }

        #endregion

        #region Destructor

        /// <summary>
        ///     Finalizes an instance of the <see cref="Segmentation" /> class.
        /// </summary>
        ~Segmentation()
        {
            Dispose(disposing: false);
        }

        #endregion

        #region MATLAB calls

        /// <summary>
        ///     Performs DiviK clustering on the specified data.
        /// </summary>
        /// <param name="dataset">Input dataset.</param>
        /// <param name="options">Configuration.</param>
        /// <returns>Segmentation result.</returns>
        /// <exception cref="System.ObjectDisposedException">thrown if this object has been disposed.</exception>
        public DivikResult Divik(IDataset dataset, DivikOptions options)
        {
            // This is needed to not to make MCR go wild
            const int numberOfOutputArgs = 2;
            var coordinates = dataset.GetRawSpacialCoordinates(is2D: true);
            var coords = new double[coordinates.GetLength(dimension: 0), coordinates.GetLength(dimension: 1)];
            for (var i = 0; i < coordinates.GetLength(dimension: 0); ++i)
            {
                for (var j = 0; j < coordinates.GetLength(dimension: 1); ++j)
                {
                    coords[i, j] = coordinates[i, j];
                }
            }

            var varargin = options.ToVarargin();
            var matlabDivikResult = _segmentation.divik(
                numberOfOutputArgs,
                dataset.GetRawIntensities(),
                coordinates,
                varargin);

            // matlabResult[0] is equal to the "partition" field in matlabResult[1], that's why we only use matlabResult[1]
            // Besides it helps to create recursive single constructor for DivikResult
            var result = new DivikResult(matlabResult: matlabDivikResult[Segmentation.DivikStructureLocation]);
            return result;
        }

        #endregion

        #region IDisposable

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(obj: this);
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _segmentation.Dispose();
                }
                _disposed = true;
            }
        }

        #endregion
    }
}
