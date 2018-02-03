﻿// Copyright (c) Six Labors and contributors.
// Licensed under the Apache License, Version 2.0.

using SixLabors.ImageSharp.Drawing.Brushes;
using SixLabors.ImageSharp.Drawing.Pens;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using SixLabors.Shapes;

namespace SixLabors.ImageSharp
{
    /// <summary>
    /// Extension methods for the <see cref="Image{TPixel}"/> type.
    /// </summary>
    public static partial class ImageExtensions
    {
        /// <summary>
        /// Draws the provided Points as a closed Linear Polygon with the provided brush at the provided thickness.
        /// </summary>
        /// <typeparam name="TPixel">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="points">The points.</param>
        /// <param name="options">The options.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> DrawPolygon<TPixel>(this IImageProcessingContext<TPixel> source, IBrush<TPixel> brush, float thickness, PointF[] points, GraphicsOptions options)
           where TPixel : struct, IPixel<TPixel>
        {
            return source.Draw(new Pen<TPixel>(brush, thickness), new Polygon(new LinearLineSegment(points)), options);
        }

        /// <summary>
        /// Draws the provided Points as a closed Linear Polygon with the provided brush at the provided thickness.
        /// </summary>
        /// <typeparam name="TPixel">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="brush">The brush.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="points">The points.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> DrawPolygon<TPixel>(this IImageProcessingContext<TPixel> source, IBrush<TPixel> brush, float thickness, PointF[] points)
           where TPixel : struct, IPixel<TPixel>
        {
            return source.Draw(new Pen<TPixel>(brush, thickness), new Polygon(new LinearLineSegment(points)));
        }

        /// <summary>
        /// Draws the provided Points as a closed Linear Polygon with the provided brush at the provided thickness.
        /// </summary>
        /// <typeparam name="TPixel">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="color">The color.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="points">The points.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> DrawPolygon<TPixel>(this IImageProcessingContext<TPixel> source, TPixel color, float thickness, PointF[] points)
           where TPixel : struct, IPixel<TPixel>
        {
            return source.DrawPolygon(new SolidBrush<TPixel>(color), thickness, points);
        }

        /// <summary>
        /// Draws the provided Points as a closed Linear Polygon with the provided brush at the provided thickness.
        /// </summary>
        /// <typeparam name="TPixel">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="color">The color.</param>
        /// <param name="thickness">The thickness.</param>
        /// <param name="points">The points.</param>
        /// <param name="options">The options.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> DrawPolygon<TPixel>(this IImageProcessingContext<TPixel> source, TPixel color, float thickness, PointF[] points, GraphicsOptions options)
           where TPixel : struct, IPixel<TPixel>
        {
            return source.DrawPolygon(new SolidBrush<TPixel>(color), thickness, points, options);
        }

        /// <summary>
        /// Draws the provided Points as a closed Linear Polygon with the provided Pen.
        /// </summary>
        /// <typeparam name="TPixel">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="points">The points.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> DrawPolygon<TPixel>(this IImageProcessingContext<TPixel> source, IPen<TPixel> pen, PointF[] points)
           where TPixel : struct, IPixel<TPixel>
        {
            return source.Draw(pen, new Polygon(new LinearLineSegment(points)), GraphicsOptions.Default);
        }

        /// <summary>
        /// Draws the provided Points as a closed Linear Polygon with the provided Pen.
        /// </summary>
        /// <typeparam name="TPixel">The type of the color.</typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="pen">The pen.</param>
        /// <param name="points">The points.</param>
        /// <param name="options">The options.</param>
        /// <returns>The <see cref="Image{TPixel}"/>.</returns>
        public static IImageProcessingContext<TPixel> DrawPolygon<TPixel>(this IImageProcessingContext<TPixel> source, IPen<TPixel> pen, PointF[] points, GraphicsOptions options)
           where TPixel : struct, IPixel<TPixel>
        {
            return source.Draw(pen, new Polygon(new LinearLineSegment(points)), options);
        }
    }
}
