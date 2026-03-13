namespace CSDB_UtopiaModel.Persistence.MapGeneration
{
    class Generator
    {
        public int Height { get; private set; }
        public int Width { get; private set; }


        public Generator(int width, int height)
        {
            if (height <= 0)
                throw new ArgumentException(nameof(height) + " must be positive.");
            if (width <= 0)
                throw new ArgumentException(nameof(width) + " must be positive.");

        }
    }
}
