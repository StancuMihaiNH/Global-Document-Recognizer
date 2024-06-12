using Azure.AI.DocumentIntelligence;

namespace Document_Intelligence.Interfaces
{
    internal interface IDocumentProcessor
    {
        #region Methods
        void ProcessDocument(AnalyzedDocument document);
        #endregion Methods
    }
}