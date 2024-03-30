namespace Sort
{
    class CArray
    {
        private readonly CVariant[][] m_oData;
        private readonly int m_nRows;
        private readonly int m_nCols;

        public CArray(int nRows, int nCols)
        {
            m_oData = new CVariant[nCols][];
            for (int iCol = 0; iCol < nCols; ++iCol)
                m_oData[iCol] = new CVariant[nRows];
            m_nRows = nRows;
            m_nCols = nCols;
        }
        public CVariant[] this[int iCol]
        {
            get { return m_oData[iCol]; }
        }

        public void ParallelUpdateAllRows()
        {
            Parallel.For(0, m_nCols, i =>
            {
                for (int j = 0; j < m_nRows; ++j)
                    m_oData[i][j].UpdateRow(j);
            });
        }
        public void UpdateAllRows()
        {
            for (int i = 0; i < m_nCols; ++i)
                for (int j = 0; j < m_nRows; ++j)
                    m_oData[i][j].UpdateRow(j);
        }

        public void Sort(int iCol)
        {
            UpdateAllRows();

            Array.Sort(m_oData[iCol]);

            for (int i = 0; i < m_nCols; ++i)
            {
                if (i != iCol)
                {
                    // Rearrange other columns to the sorted order in the swap space
                    for (int j = 0, j2; j < m_nRows; ++j)
                    {
                        j2 = m_oData[iCol][j].Row;
                        if (j == j2)  // didn't move, skip copy optimization
                            continue;
                        m_oData[i][j].Copy(m_oData[i][j2], 0, 1);
                    }

                    // Move the swapped data to main to complete index reordering
                    for (int j = 0; j < m_nRows; ++j)
                    {
                        m_oData[i][j].Copy(m_oData[i][j], 1, 0);
                        m_oData[i][j].UpdateRow(j);
                    }
                }
            }

            // Finally, reindex sorted column
            for (int j = 0; j < m_nRows; ++j)
                m_oData[iCol][j].UpdateRow(j);
        }

        public void ParallelSort(int iCol)
        {
            // Index the row data
            ParallelUpdateAllRows();

            Array.Sort(m_oData[iCol]);

            Parallel.For(0, m_nCols, i =>
            {
                if (i != iCol)
                {
                    // Rearrange other columns to the sorted order in the swap space
                    for (int j = 0, j2; j < m_nRows; ++j)
                    {
                        j2 = m_oData[iCol][j].Row;
                        if (j == j2) // didn't move, skip copy optimization
                            continue;
                        m_oData[i][j].Copy(m_oData[i][j2], 0, 1);
                    }

                    // Move the swapped data to main to complete index reordering
                    for (int j = 0; j < m_nRows; ++j)
                    {
                        m_oData[i][j].Copy(m_oData[i][j], 1, 0);
                        m_oData[i][j].UpdateRow(j);
                    }
                }
            });

            // Finally, reindex sorted column
            for (int j = 0; j < m_nRows; ++j)
                m_oData[iCol][j].UpdateRow(j);
        }
        public int ColLength
        {
            get { return m_nCols; }
        }
        public int RowLength
        {
            get { return m_nRows; }
        }
    }
}
