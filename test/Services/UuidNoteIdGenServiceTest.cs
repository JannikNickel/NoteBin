using NoteBin.Services;
using System.Collections.Generic;
using System.Linq;

namespace NoteBin.Test.Services
{
    [TestClass]
    public sealed class UuidNoteIdGenServiceTest
    {
        private UuidNoteIdGenService service = new UuidNoteIdGenService();

        [TestMethod, DataRow(100000)]
        public void GeneratedIds_AreUnique(int samples)
        {
            HashSet<string> results = new HashSet<string>();
            for(int i = 0;i < samples;i++)
            {
                results.Add(service.GenerateId());
            }

            int diff = samples - results.Count;
            Assert.IsTrue(diff < samples * 0.00001);
        }

        [TestMethod, DataRow(100000)]
        public void CharacterDistribution_IsUniform(int samples)
        {
            Dictionary<char, int> results = new Dictionary<char, int>();
            for(int i = 0;i < samples;i++)
            {
                string id = service.GenerateId();
                for(int k = 0;k < id.Length;k++)
                {
                    results.TryAdd(id[k], 0);
                    results[id[k]] += 1;
                }
            }

            int maxFreq = results.Values.Max();
            int minFreq = results.Values.Min();
            int diff = maxFreq - minFreq;
            Assert.IsTrue(diff < 0.05 * samples, $"Distribution is not uniform. max = {maxFreq}, min = {minFreq}, diff = {diff}");
        }

        [TestMethod, DataRow(1000)]
        public void GeneratedIds_HaveCorrectLength(int samples)
        {
            for(int i = 0;i < samples;i++)
            {
                string id = service.GenerateId();
                Assert.AreEqual(Constants.NoteIdLength, id.Length, $"Generated ID does not have the correct length. ID: {id}");
            }
        }
    }
}
