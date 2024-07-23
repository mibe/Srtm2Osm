namespace Brejc.DemLibrary
{
    public delegate void NewIsohypseCallback (Isohypse isohypse);

    /// <summary>
    /// Defines a method for calculating isohypses (elevation contours) on a digital elevation mode.
    /// </summary>
    public interface IIsopletingAlgorithm
    {
        /// <summary>
        /// Gets or sets the activity logger.
        /// </summary>
        /// <value>The activity logger.</value>
        IActivityLogger ActivityLogger { get; set;}

        //IVisualizer Visualizer { get; set;}

        /// <summary>
        /// Returns a collection of isohypses for the specified digital elevation model using a specified elevation step.
        /// </summary>
        /// <param name="dem">The digital elevation model.</param>
        /// <param name="elevationStep">The step (in meters) to use when calculating isohypses.</param>
        /// <param name="setMinElevation">The first elevation (in meters) to use when calculating isohypses (if not null).</param>
        /// <returns>A collection of isohypses stored in the <see cref="IsohypseCollection"/> object.</returns>
        IsohypseCollection Isoplete (IRasterDigitalElevationModel dem, double elevationStep, double? setMinElevation);

        void Isoplete (IRasterDigitalElevationModel dem, double elevationStep, double? setMinElevation, NewIsohypseCallback callback);
    }
}
