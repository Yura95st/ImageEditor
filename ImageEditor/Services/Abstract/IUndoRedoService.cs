namespace ImageEditor.Services.Abstract
{
    public interface IUndoRedoService<T>
    where T : class
    {
        /// <summary>Adds the entry.</summary>
        /// <param name="newEntry">The new entry.</param>
        void AddEntry(T newEntry);

        /// <summary>Determines whether the redo operation can be performed.</summary>
        /// <returns>True - if redo operation can be performed, otherwise - false</returns>
        bool CanRedo();

        /// <summary>Determines whether the undo operation can be performed.</summary>
        /// <returns>True - if undo operation can be performed, otherwise - false</returns>
        bool CanUndo();

        /// <summary>Clears all entries.</summary>
        void Clear();

        /// <summary>Gets the last added entry.</summary>
        /// <returns>The last added entry</returns>
        T GetLastEntry();

        /// <summary>Performs the redo operation.</summary>
        /// <exception cref="System.InvalidOperationException">The redo operation is forbidden.</exception>
        void Redo();

        /// <summary>Performs the undo operation.</summary>
        /// <exception cref="System.InvalidOperationException">The undo operation is forbidden.</exception>
        void Undo();
    }
}