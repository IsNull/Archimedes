using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.WPF.FlowDocuments
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Threading;
    using System.Windows.Documents;
    using System.Windows.Documents.Serialization;
    using System.Windows.Markup;

    namespace Fohjin.XamlLibrary
    {
        /// <summary>
        /// Class that will take care of converting a Xaml WPF FlowDocument to a XPS document
        /// </summary>
        public class XamlToXps
        {
            /// <summary>
            /// Holds an internal copy of the Loaded Xaml
            /// </summary>
            private IDocumentPaginatorSource _flowDocument;
            /// <summary>
            /// This Dictionary is used to perform string replacements in the source Xaml, 
            /// I use this f.ex. to replace an external default reference to an actual XML 
            /// file location
            /// </summary>
            private Dictionary<string, string> _stringReplacement;

            /// <summary>
            /// Initializes a new instance of the <see cref="XamlToXps"/> class.
            /// </summary>
            public XamlToXps() {
                _stringReplacement = new Dictionary<string, string>();
            }

            /// <summary>
            /// Gets or sets the string replacement dictionary later to be used for string 
            /// replacements in the Xaml source.
            /// </summary>
            /// <value>The string replacement dictionary.</value>
            public Dictionary<string, string> StringReplacement {
                set {
                    _stringReplacement = value;
                }
                get {
                    return _stringReplacement;
                }
            }

            /// <summary>
            /// Loads the source Xaml using a file location.
            /// At this stage there will be automatic string replacement.
            /// </summary>
            /// <param name="xamlFileName">Name of the xaml file.</param>
            public void LoadXaml(string xamlFileName) {
                using (FileStream inputStream = File.OpenRead(xamlFileName)) {
                    LoadXaml(inputStream);
                }
            }
            /// <summary>
            /// Loads the source Xaml using a Stream.
            /// At this stage there will be automatic string replacement.
            /// </summary>
            /// <param name="xamlFileStream">The xaml file stream.</param>
            public void LoadXaml(Stream xamlFileStream) {
                ParserContext pc = new ParserContext
                {
                    BaseUri = new Uri(Environment.CurrentDirectory + "/")
                };
                object newDocument = XamlReader.Load(ReplaceStringsInXaml(xamlFileStream), pc);

                if (newDocument == null)
                    throw new Exception("Invalid Xaml, could not be parsed");

                if (newDocument is IDocumentPaginatorSource)
                    LoadXaml(newDocument as IDocumentPaginatorSource);
            }
            /// <summary>
            /// Loads the source Xaml in the form of a complete FlowDocument. 
            /// At this stage there is no automatic string replacement.
            /// </summary>
            /// <param name="flowDocument">The flow document.</param>
            public void LoadXaml(IDocumentPaginatorSource flowDocument) {
                _flowDocument = flowDocument;
                FlushDispatcher();
            }

            /// <summary>
            /// Saves the prepared FlowDocument to a XPS file format.
            /// In this library there is only the XPS serializer, but in the Microsoft 
            /// example there are several, I still have to investigate how to get the others.
            /// </summary>
            /// <param name="fileName">Name of the file.</param>
            public void Save(string fileName) {
                DeleteOldFile(fileName);
                FlowDocument flowDocument = _flowDocument as FlowDocument;

                SerializerProvider serializerProvider = new SerializerProvider();
                SerializerDescriptor selectedPlugIn = null;
                foreach (SerializerDescriptor serializerDescriptor in serializerProvider.InstalledSerializers) {
                    if (!serializerDescriptor.IsLoadable || !fileName.EndsWith(serializerDescriptor.DefaultFileExtension))
                        continue;
                    selectedPlugIn = serializerDescriptor;
                    break;
                }

                if (selectedPlugIn != null) {
                    using (Stream package = File.Create(fileName)) {
                        SerializerWriter serializerWriter = serializerProvider.CreateSerializerWriter(selectedPlugIn, package);
                        IDocumentPaginatorSource idoc = flowDocument;
                        if (idoc != null)
                            serializerWriter.Write(idoc.DocumentPaginator, null);
                        package.Close();
                    }
                } else
                    throw new Exception("No Serializer found for your requested output format");
            }

            /// <summary>
            /// Deletes the old output file if it exists.
            /// </summary>
            /// <param name="fileName">Name of the file.</param>
            private static void DeleteOldFile(string fileName) {
                if (File.Exists(fileName)) {
                    File.Delete(fileName);
                }
            }

            /// <summary>
            /// Replaces the Key Value Pairs in the Xaml source, this is an other way of data 
            /// binding, but I prefere the build-in way.
            /// </summary>
            /// <param name="xamlFileStream">The xaml file stream.</param>
            /// <returns></returns>
            private Stream ReplaceStringsInXaml(Stream xamlFileStream) {
                string rawXamlText;
                xamlFileStream.Seek(0, SeekOrigin.Begin);
                using (StreamReader streamReader = new StreamReader(xamlFileStream)) {
                    rawXamlText = streamReader.ReadToEnd();
                }
                foreach (KeyValuePair<string, string> keyValuePair in _stringReplacement) {
                    rawXamlText = rawXamlText.Replace(keyValuePair.Key, keyValuePair.Value);
                }
                return new MemoryStream(new UTF8Encoding().GetBytes(rawXamlText));
            }

            /// <summary>
            /// Have to figure out what this does and why it works, but not including 
            /// this and calling it wil actually fail the databinding process
            /// </summary>
            private static void FlushDispatcher() {
                FlushDispatcher(Dispatcher.CurrentDispatcher);
            }

            /// <summary>
            /// Have to figure out what this does and why it works, but not including 
            /// this and calling it wil actually fail the databinding process
            /// </summary>
            /// <param name="ctx"></param>
            private static void FlushDispatcher(Dispatcher ctx) {
                FlushDispatcher(ctx, DispatcherPriority.SystemIdle);
            }

            /// <summary>
            /// Have to figure out what this does and why it works, but not including 
            /// this and calling it wil actually fail the databinding process
            /// </summary>
            /// <param name="ctx"></param>
            /// <param name="priority"></param>
            private static void FlushDispatcher(Dispatcher ctx, DispatcherPriority priority) {
                ctx.Invoke(priority, new DispatcherOperationCallback(delegate { return null; }), null);
            }
        }
    }
}
