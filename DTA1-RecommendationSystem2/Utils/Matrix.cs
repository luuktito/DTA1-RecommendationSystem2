using System.Collections.Generic;

namespace DTA1_RecommendationSystem2.Utils
{
    class Matrix<T>
    {
        private T[,] matrix;
        private Dictionary<int, int> loopUpTable = new Dictionary<int, int>();

        public Matrix(int sizeX, int sizeY, Dictionary<int, int> loopUpTable)
        {
            matrix = new T[sizeX, sizeY];
            this.loopUpTable = loopUpTable;
        }

        public T this[int x, int y]
        {
            get
            {
                return matrix[loopUpTable[x], loopUpTable[y]];
            }
            set
            {
               matrix[loopUpTable[x], loopUpTable[y]] = value;
            }
        }
    }
}
