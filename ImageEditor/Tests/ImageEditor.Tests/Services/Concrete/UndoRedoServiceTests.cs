namespace ImageEditor.Tests.Services.Concrete
{
    using System;

    using ImageEditor.Services.Abstract;
    using ImageEditor.Services.Concrete;

    using NUnit.Framework;

    [TestFixture]
    public class UndoRedoServiceTests
    {
        private IUndoRedoService<object> _undoRedoService;

        [Test]
        public void AddEntry_EntryIsNull_ThrowsArgumentNullExeption()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => this._undoRedoService.AddEntry(null));
        }

        [Test]
        public void AddEntry_EntryIsValid_AddsTheEntryAndSetsItAsTheLastOne()
        {
            // Arrange
            object entry = new object();

            // Act
            this._undoRedoService.AddEntry(entry);

            // Assert
            object lastEntry = this._undoRedoService.GetLastEntry();

            Assert.AreEqual(entry, lastEntry);
        }

        [Test]
        public void CanRedo_EntriesListIsEmpty_ReturnsFalse()
        {
            // Act & Assert
            Assert.IsFalse(this._undoRedoService.CanRedo());
        }

        [Test]
        public void CanRedo_RedoOperationIsForbidden_ReturnsFalse()
        {
            // Arrange
            object entry = new object();

            this._undoRedoService.AddEntry(entry);

            // Act & Assert
            Assert.IsFalse(this._undoRedoService.CanRedo());
        }

        [Test]
        public void CanRedo_UndoOperationIsAllowed_ReturnsTrue()
        {
            // Arrange
            object entry = new object();

            this._undoRedoService.AddEntry(entry);
            this._undoRedoService.Undo();

            // Act & Assert
            Assert.IsTrue(this._undoRedoService.CanRedo());
        }

        [Test]
        public void CanUndo_EntriesListIsEmpty_ReturnsFalse()
        {
            // Act & Assert
            Assert.IsFalse(this._undoRedoService.CanUndo());
        }

        [Test]
        public void CanUndo_UndoOperationIsAllowed_ReturnsTrue()
        {
            // Arrange
            object entry = new object();

            this._undoRedoService.AddEntry(entry);

            // Act & Assert
            Assert.IsTrue(this._undoRedoService.CanUndo());
        }

        [Test]
        public void CanUndo_UndoOperationIsForbidden_ReturnsFalse()
        {
            // Arrange
            object entry = new object();

            this._undoRedoService.AddEntry(entry);
            this._undoRedoService.Undo();

            // Act & Assert
            Assert.IsFalse(this._undoRedoService.CanUndo());
        }

        [Test]
        public void Clear_PerformsClearOperation()
        {
            // Arrange
            object entryOne = new object();
            object entryTwo = new object();

            this._undoRedoService.AddEntry(entryOne);
            this._undoRedoService.AddEntry(entryTwo);

            this._undoRedoService.Undo();

            // Act
            this._undoRedoService.Clear();

            // Assert
            Assert.AreEqual(null, this._undoRedoService.GetLastEntry());

            Assert.IsFalse(this._undoRedoService.CanUndo());
            Assert.IsFalse(this._undoRedoService.CanRedo());
        }

        [Test]
        public void GetLastEntry_EntriesListIsEmpty_ReturnsNull()
        {
            // Act
            object lastEntry = this._undoRedoService.GetLastEntry();

            // Assert
            Assert.IsNull(lastEntry);
        }

        [Test]
        public void GetLastEntry_EntriesListIsNotEmpty_ReturnsValidEntry()
        {
            // Arrange
            object entry = new object();

            this._undoRedoService.AddEntry(entry);

            // Act
            object lastEntry = this._undoRedoService.GetLastEntry();

            // Assert
            Assert.AreEqual(entry, lastEntry);
        }

        [Test]
        public void Redo_EntriesListIsEmpty_ThrowsInvalidOperationException()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => this._undoRedoService.Redo());
        }

        [Test]
        public void Redo_RedoOperationIsAllowed_PerformsRedoOperation()
        {
            // Arrange
            object entryOne = new object();

            this._undoRedoService.AddEntry(entryOne);
            this._undoRedoService.Undo();

            // Act
            this._undoRedoService.Redo();

            // Assert
            Assert.AreEqual(entryOne, this._undoRedoService.GetLastEntry());
        }

        [Test]
        public void Redo_RedoOperationIsForbidden_ThrowsInvalidOperationException()
        {
            // Arrange
            object entry = new object();

            this._undoRedoService.AddEntry(entry);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => this._undoRedoService.Redo());
        }

        [SetUp]
        public void SetUp()
        {
            this._undoRedoService = new UndoRedoService<object>();
        }

        [Test]
        public void Undo_EntriesListIsEmpty_ThrowsInvalidOperationException()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => this._undoRedoService.Undo());
        }

        [Test]
        public void Undo_UndoOperationIsAllowed_PerformsUndoOperation()
        {
            // Arrange
            object entryOne = new object();
            object entryTwo = new object();

            this._undoRedoService.AddEntry(entryOne);
            this._undoRedoService.AddEntry(entryTwo);

            // Act
            this._undoRedoService.Undo();

            // Assert
            Assert.AreEqual(entryOne, this._undoRedoService.GetLastEntry());
        }

        [Test]
        public void Undo_UndoOperationIsForbidden_ThrowsInvalidOperationException()
        {
            // Arrange
            object entry = new object();

            this._undoRedoService.AddEntry(entry);
            this._undoRedoService.Undo();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => this._undoRedoService.Undo());
        }
    }
}