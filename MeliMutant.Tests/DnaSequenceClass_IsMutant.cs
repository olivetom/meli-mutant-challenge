using NUnit.Framework;
using meli;

namespace MeliMutant.Tests
{
    public class Tests
    {
        private DnaSequence dnaSequence1, dnaSequence2, dnaSequence3, dnaSequence4;
        private string[] Mutante1 = new string[]
                          { "A T G C G A".Replace(" ",""),
                            "C A G T G C".Replace(" ",""),
                            "T T A T G T".Replace(" ",""),
                            "A G A A G C".Replace(" ",""),
                            "C C C C T A".Replace(" ",""),
                            "T C A C T G".Replace(" ","")  };

        private string[] Mutante2 = new string[]
                          { "T T G C G A".Replace(" ",""),
                            "C A G T G C".Replace(" ",""),
                            "T T A T G T".Replace(" ",""),
                            "A G A A G C".Replace(" ",""),
                            "C C C C A A".Replace(" ",""),
                            "T C A C T G".Replace(" ","")  };

        private string[] Mutante3 = new string[]
                          { "T T G T G A".Replace(" ",""),
                            "C A T T G C".Replace(" ",""),
                            "T T A T G T".Replace(" ",""),
                            "T G A A G C".Replace(" ",""),
                            "C C C C A A".Replace(" ",""),
                            "T C A C T G".Replace(" ","")  };

        private string[] Human1 = new string[]
                          { "A T G T G A".Replace(" ",""),
                            "C C T T G C".Replace(" ",""),
                            "T T A T A T".Replace(" ",""),
                            "T G A A G C".Replace(" ",""),
                            "C A C C A A".Replace(" ",""),
                            "T C A C T G".Replace(" ","")  };

        [SetUp]
        public void Setup()
        {
            dnaSequence1 = new DnaSequence(Mutante1);
            dnaSequence2 = new DnaSequence(Mutante2);
            dnaSequence3 = new DnaSequence(Mutante3);
            dnaSequence4 = new DnaSequence(Human1);
        }

        [Test]
        public void IsMutant1()
        {
            var result = dnaSequence1.IsMutant;

            Assert.IsTrue(result, "dna should be mutant");
        }

        [Test]
        public void IsMutant2()
        {
            var result = dnaSequence2.IsMutant;

            Assert.IsTrue(result, "dna should be mutant");
        }

        [Test]
        public void IsMutant3()
        {
            var result = dnaSequence3.IsMutant;

            Assert.IsTrue(result, "dna should be mutant");
        }

        [Test]
        public void IsHuman()
        {
            var result = dnaSequence4.IsMutant;

            Assert.IsFalse(result, "dna should be human");
        }
    }
}
