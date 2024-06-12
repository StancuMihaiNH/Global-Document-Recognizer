using Azure.AI.DocumentIntelligence;
using Document_Intelligence.Interfaces;
using System;

namespace Document_Intelligence.Services
{
    internal class InvoiceProcessor : IDocumentProcessor
    {
        #region Properties
        private readonly DocumentIntelligenceClientAdapter _client;
        #endregion Properties

        #region Constructors
        public InvoiceProcessor(DocumentIntelligenceClientAdapter client)
        {
            _client = client;
        }
        #endregion Constructors

        #region Methods
        public void ProcessDocument(AnalyzedDocument document)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DocumentField> field in document.Fields)
            {
                Console.WriteLine($"{field.Key}: {field.Value.Content} (Confidence: {field.Value.Confidence})");
            }
        }
        #endregion Methods
    }
}