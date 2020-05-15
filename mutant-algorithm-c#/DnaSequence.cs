using System.Linq;

namespace meli
{
    class Cell
    {
        public char Element { get; set; }
        public int RowIndex { get; set; }
        public int ColIndex { get; set; }
        public int DiagIndex { get; set; }
        public int RevDiagIndex { get; set; }
    }

    public class DnaSequence
    {
        string[] Dna;
        public bool IsMutant { get; private set; }

        const int dnaSeqLength = 4;
        readonly char[] DnaElements = { 'A', 'C', 'G', 'T' };
        const int MinCountForMutant = 2;


        public DnaSequence(string[] dna)
        {
            SetDna(dna);
        }

        public void SetDna(string[] dna)
        {
            Dna = dna;
            if (MutantCheck()) IsMutant = true;
            else IsMutant = false;
        }

        private bool MutantCheck()
        {
            var flattenDna = Dna
                    .SelectMany((row, rowIndex) => row
                        .Select((letter, colIndex) => new Cell
                        {
                            Element = letter,
                            RowIndex = rowIndex,
                            ColIndex = colIndex,
                            DiagIndex = rowIndex - colIndex,
                            RevDiagIndex = rowIndex + colIndex
                        }));

            var mutantCount = 0;

            var dnaList = flattenDna.GroupBy(cell => cell.RowIndex);

            dnaList = dnaList.Concat(flattenDna.GroupBy(cell => cell.ColIndex));

            dnaList = dnaList.Concat(flattenDna.GroupBy(cell => cell.DiagIndex));

            dnaList = dnaList.Concat(flattenDna.GroupBy(cell => cell.RevDiagIndex));

            foreach (var sequence in dnaList)
            {
                mutantCount += sequence
                    .GroupWhile((current, next) =>
                        current.Element == next.Element && DnaElements.Contains(current.Element))
                    .Where(group =>
                        group.Count() >= dnaSeqLength)
                    .Count();
                if (mutantCount >= MinCountForMutant)
                    return true;
            };

            return false;
        }
    }
}
