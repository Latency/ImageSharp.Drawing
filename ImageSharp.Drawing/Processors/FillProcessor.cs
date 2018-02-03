﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Drawing.Brushes;
using SixLabors.ImageSharp.Drawing.Brushes.Processors;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace SixLabors.ImageSharp.Drawing.Processors
{
    /// <summary>
    /// Using the bursh as a source of pixels colors blends the brush color with source.
    /// </summary>
    /// <typeparam name="TPixel">The pixel format.</typeparam>
    internal class FillProcessor<TPixel> : ImageProcessor<TPixel>
        where TPixel : struct, IPixel<TPixel>
    {
        /// <summary>
        /// The brush.
        /// </summary>
        private readonly IBrush<TPixel> brush;
        private readonly GraphicsOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="FillProcessor{TPixel}"/> class.
        /// </summary>
        /// <param name="brush">The brush to source pixel colors from.</param>
        /// <param name="options">The options</param>
        public FillProcessor(IBrush<TPixel> brush, GraphicsOptions options)
        {
            this.brush = brush;
            this.options = options;
        }

        /// <inheritdoc/>
        protected override void OnApply(ImageFrame<TPixel> source, Rectangle sourceRectangle, Configuration configuration)
        {
            int startX = sourceRectangle.X;
            int endX = sourceRectangle.Right;
            int startY = sourceRectangle.Y;
            int endY = sourceRectangle.Bottom;

            // Align start/end positions.
            int minX = Math.Max(0, startX);
            int maxX = Math.Min(source.Width, endX);
            int minY = Math.Max(0, startY);
            int maxY = Math.Min(source.Height, endY);

            // Reset offset if necessary.
            if (minX > 0)
            {
                startX = 0;
            }

            if (minY > 0)
            {
                startY = 0;
            }

            int width = maxX - minX;

            using (var amount = new Buffer<float>(width))
            using (BrushApplicator<TPixel> applicator = this.brush.CreateApplicator(source, sourceRectangle, this.options))
            {
                    for (int i = 0; i < width; i++)
                    {
                        amount[i] = this.options.BlendPercentage;
                    }

                    Parallel.For(
                    minY,
                    maxY,
                    configuration.ParallelOptions,
                    y =>
                    {
                        int offsetY = y - startY;
                        int offsetX = minX - startX;

                        applicator.Apply(amount, offsetX, offsetY);
                    });
            }
        }
    }
}