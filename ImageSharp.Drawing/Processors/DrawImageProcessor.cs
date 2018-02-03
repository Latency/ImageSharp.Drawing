﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using System;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Helpers;
using SixLabors.ImageSharp.Memory;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace SixLabors.ImageSharp.Drawing.Processors
{
    /// <summary>
    /// Combines two images together by blending the pixels.
    /// </summary>
    /// <typeparam name="TPixel">The pixel format.</typeparam>
    internal class DrawImageProcessor<TPixel> : ImageProcessor<TPixel>
        where TPixel : struct, IPixel<TPixel>
    {
        private readonly PixelBlender<TPixel> blender;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawImageProcessor{TPixel}"/> class.
        /// </summary>
        /// <param name="image">The image to blend with the currently processing image.</param>
        /// <param name="size">The size to draw the blended image.</param>
        /// <param name="location">The location to draw the blended image.</param>
        /// <param name="options">The opacity of the image to blend. Between 0 and 100.</param>
        public DrawImageProcessor(Image<TPixel> image, Size size, Point location, GraphicsOptions options)
        {
            Guard.MustBeBetweenOrEqualTo(options.BlendPercentage, 0, 1, nameof(options.BlendPercentage));
            this.Image = image;
            this.Size = size;
            this.Alpha = options.BlendPercentage;
            this.blender = PixelOperations<TPixel>.Instance.GetPixelBlender(options.BlenderMode);
            this.Location = location;
        }

        /// <summary>
        /// Gets the image to blend.
        /// </summary>
        public Image<TPixel> Image { get; }

        /// <summary>
        /// Gets the alpha percentage value.
        /// </summary>
        public float Alpha { get; }

        /// <summary>
        /// Gets the size to draw the blended image.
        /// </summary>
        public Size Size { get; }

        /// <summary>
        /// Gets the location to draw the blended image.
        /// </summary>
        public Point Location { get; }

        /// <inheritdoc/>
        protected override void OnApply(ImageFrame<TPixel> source, Rectangle sourceRectangle, Configuration configuration)
        {
            Image<TPixel> disposableImage = null;
            Image<TPixel> targetImage = this.Image;

            try
            {
                if (targetImage.Size() != this.Size)
                {
                    targetImage = disposableImage = this.Image.Clone(x => x.Resize(this.Size.Width, this.Size.Height));
                }

                // Align start/end positions.
                Rectangle bounds = targetImage.Bounds();
                int minX = Math.Max(this.Location.X, sourceRectangle.X);
                int maxX = Math.Min(this.Location.X + bounds.Width, sourceRectangle.Width);
                maxX = Math.Min(this.Location.X + this.Size.Width, maxX);
                int targetX = minX - this.Location.X;

                int minY = Math.Max(this.Location.Y, sourceRectangle.Y);
                int maxY = Math.Min(this.Location.Y + bounds.Height, sourceRectangle.Bottom);

                maxY = Math.Min(this.Location.Y + this.Size.Height, maxY);

                int width = maxX - minX;
                using (var amount = new Buffer<float>(width))
                {
                    for (int i = 0; i < width; i++)
                    {
                        amount[i] = this.Alpha;
                    }

                    Parallel.For(
                        minY,
                        maxY,
                        configuration.ParallelOptions,
                        y =>
                            {
                                Span<TPixel> background = source.GetPixelRowSpan(y).Slice(minX, width);
                                Span<TPixel> foreground = targetImage.GetPixelRowSpan(y - this.Location.Y).Slice(targetX, width);
                                this.blender.Blend(background, background, foreground, amount);
                            });
                }
            }
            finally
            {
                disposableImage?.Dispose();
            }
        }
    }
}