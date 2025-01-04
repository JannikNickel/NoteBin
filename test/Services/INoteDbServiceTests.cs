using NoteBin.Models;
using NoteBin.Models.API;
using NoteBin.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoteBin.Test.Services
{
    public abstract class INoteDbServiceTests<T> where T : INoteDbService
    {
        protected INoteIdGenService idGenService = null!;
        protected INoteContentService contentService = null!;
        protected T service = default!;

        private static NoteCreateRequest NoteData0 => TestHelper.NoteCreateRequest("note 0", null, "plain", "content 0");
        private static NoteCreateRequest NoteData1 => TestHelper.NoteCreateRequest("note 1", null, "plain", "content 1");

        private static IEnumerable<object?[]> NoteCreateTestData()
        {
            yield return new object?[] { TestHelper.NoteCreateRequest("name", null, "plain", "note with name"), null };
            yield return new object?[] { TestHelper.NoteCreateRequest("", null, "plain", "note with empty name"), null };
            yield return new object?[] { TestHelper.NoteCreateRequest(null, null, "plain", "note with missing name"), null };
            yield return new object?[] { TestHelper.NoteCreateRequest("forked", TestHelper.FakeNoteId(), "csharp", "forked note"), null };
            yield return new object?[] { TestHelper.NoteCreateRequest("owned", null, "csharp", "owned note"), "username" };
            yield return new object?[] { TestHelper.NoteCreateRequest("forked and owned", TestHelper.FakeNoteId(), "csharp", "forked and owned note"), "username" };
        }

        [TestMethod]
        [DynamicData(nameof(NoteCreateTestData), DynamicDataSourceType.Method)]
        public async Task SaveNote_MatchesInput(NoteCreateRequest request, string? owner)
        {
            Note? result = await service.SaveNote(request, TestHelper.FakeUser(owner));

            Assert.IsNotNull(result);
            Assert.AreEqual(request.Name, result.Name);
            Assert.AreEqual(request.Fork, result.Fork);
            Assert.AreEqual(request.Syntax, result.Syntax);
            Assert.AreEqual(owner, result.Owner);
        }

        [TestMethod]
        [DynamicData(nameof(NoteCreateTestData), DynamicDataSourceType.Method)]
        public async Task GetNote_MatchesSaved(NoteCreateRequest request, string? owner)
        {
            Note? savedNote = await service.SaveNote(request, TestHelper.FakeUser(owner));
            Assert.IsNotNull(savedNote);

            Note? retrievedNote = await service.GetNote(savedNote.Id);
            Assert.IsNotNull(retrievedNote);
            Assert.AreEqual(savedNote.Id, retrievedNote.Id);
            Assert.AreEqual(savedNote.Name, retrievedNote.Name);
            Assert.AreEqual(savedNote.Owner, retrievedNote.Owner);
            Assert.AreEqual(savedNote.Fork, retrievedNote.Fork);
            Assert.AreEqual(savedNote.Syntax, retrievedNote.Syntax);
            Assert.AreEqual(request.Content, retrievedNote.Content);
        }

        [TestMethod]
        public async Task GetNote_NoNotes()
        {
            Note? retrievedNote = await service.GetNote(TestHelper.FakeNoteId());
            Assert.IsNull(retrievedNote);
        }

        [TestMethod]
        public async Task GetNotes_NoNotes()
        {
            (List<Note> notes, long total) = await service.GetNotes(0, 10);
            Assert.AreEqual(0, total);
            Assert.AreEqual(0, notes.Count);
        }

        [TestMethod]
        public async Task GetNotes_SingleNote()
        {
            await service.SaveNote(NoteData0, null);

            (List<Note> notes, long total) = await service.GetNotes(0, 10);
            Assert.AreEqual(1, total);
            Assert.AreEqual(1, notes.Count);
            Assert.AreEqual(NoteData0.Name, notes[0].Name);
            Assert.AreEqual(NoteData0.Content, notes[0].Content);
        }

        [TestMethod]
        public async Task GetNotes_MultipleNotes()
        {
            await service.SaveNote(NoteData0, null);
            await service.SaveNote(NoteData1, null);

            (List<Note> notes, long total) = await service.GetNotes(0, 10);
            Assert.AreEqual(2, total);
            Assert.AreEqual(2, notes.Count);
        }

        [TestMethod]
        public async Task GetNotes_WithOffset()
        {
            await service.SaveNote(NoteData0, null);
            await service.SaveNote(NoteData1, null);

            (List<Note> notes, long total) = await service.GetNotes(1, 10);
            Assert.AreEqual(2, total);
            Assert.AreEqual(1, notes.Count);
            Assert.AreEqual(NoteData0.Name, notes[0].Name);
            Assert.AreEqual(NoteData0.Content, notes[0].Content);
        }

        [TestMethod]
        public async Task GetNotes_WithAmount()
        {
            await service.SaveNote(NoteData0, null);
            await service.SaveNote(NoteData1, null);

            (List<Note> notes, long total) = await service.GetNotes(0, 1);
            Assert.AreEqual(2, total);
            Assert.AreEqual(1, notes.Count);
            Assert.AreEqual(NoteData1.Name, notes[0].Name);
            Assert.AreEqual(NoteData1.Content, notes[0].Content);
        }

        [TestMethod]
        public async Task GetNotes_WithUserFilter()
        {
            await service.SaveNote(NoteData0, TestHelper.FakeUser("user0"));
            await service.SaveNote(NoteData1, TestHelper.FakeUser("user1"));

            (List<Note> notes, long total) = await service.GetNotes(0, 10, "user0");
            Assert.AreEqual(1, total);
            Assert.AreEqual(1, notes.Count);
            Assert.AreEqual(NoteData0.Name, notes[0].Name);
            Assert.AreEqual(NoteData0.Content, notes[0].Content);
        }

        [TestMethod]
        public async Task GetNotes_WithContentFilter()
        {
            await service.SaveNote(NoteData0, null);
            await service.SaveNote(NoteData1, null);

            (List<Note> notes, long total) = await service.GetNotes(0, 10, null, NoteData1.Name);
            Assert.AreEqual(1, total);
            Assert.AreEqual(1, notes.Count);
            Assert.AreEqual(NoteData1.Name, notes[0].Name);
            Assert.AreEqual(NoteData1.Content, notes[0].Content);
        }
    }
}
