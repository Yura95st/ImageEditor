namespace ImageEditor.Services.Concrete
{
    using System;
    using System.Collections.Generic;

    using ImageEditor.Services.Abstract;
    using ImageEditor.Utils;

    public class UndoRedoService<T> : IUndoRedoService<T>
    where T : class
    {
        private readonly List<T> _entriesList;

        private int _lastEntryIndex;

        public UndoRedoService()
        {
            this._entriesList = new List<T>();

            this.Clear();
        }

        #region IUndoRedoService<T> Members

        public void AddEntry(T newEntry)
        {
            Guard.NotNull(newEntry, "newEntry");

            this._lastEntryIndex++;

            this._entriesList.RemoveRange(this._lastEntryIndex, this._entriesList.Count - this._lastEntryIndex);

            this._entriesList.Add(newEntry);
        }

        public bool CanRedo()
        {
            bool result = this._entriesList.Count > 0 && this._lastEntryIndex + 1 < this._entriesList.Count;

            return result;
        }

        public bool CanUndo()
        {
            bool result = this._entriesList.Count > 0 && this._lastEntryIndex >= 0;

            return result;
        }

        public void Clear()
        {
            this._entriesList.Clear();

            this._lastEntryIndex = -1;
        }

        public T GetLastEntry()
        {
            T entry = null;

            if (this._lastEntryIndex >= 0 && this._lastEntryIndex < this._entriesList.Count)
            {
                entry = this._entriesList[this._lastEntryIndex];
            }

            return entry;
        }

        public void Redo()
        {
            if (!this.CanRedo())
            {
                throw new InvalidOperationException("Redo operation is forbidden.");
            }

            this._lastEntryIndex++;
        }

        public void Undo()
        {
            if (!this.CanUndo())
            {
                throw new InvalidOperationException("Redo operation is forbidden.");
            }

            this._lastEntryIndex--;
        }

        #endregion
    }
}