using NoteBin.Services;
using System;
using System.Collections.Generic;

namespace NoteBin.Test
{
    [TestClass]
    public sealed class UuidNoteIdGenServiceTest
    {
        [TestMethod]
        public void UniquenessRatio_Almost_One()
        {
            UuidNoteIdGenService service = new UuidNoteIdGenService();

            const int tests = 1000000;
            HashSet<string> results = new HashSet<string>();
            for(int i = 0;i < tests;i++)
            {
                results.Add(service.GenerateId());
            }

            Assert.AreEqual(1.0, results.Count / (double)tests, 0.001);
        }
    }
}
