using System.Management.Automation;
using System.Management.Automation.Language;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

namespace Base64
{
    class EncodingTransformAttribute : ArgumentTransformationAttribute
    {
        public EncodingTransformAttribute() { }
        internal static Dictionary<string, Encoding> StandardEncoding = new Dictionary<string, Encoding>()
        {
            { "ASCII", Encoding.ASCII },
            { "UnicodeBE", Encoding.BigEndianUnicode },
            { "Unicode", Encoding.Unicode },
            { "UTF32", Encoding.UTF32 },
            { "UTF7", Encoding.UTF7 },
            { "UTF8", new UTF8Encoding(encoderShouldEmitUTF8Identifier: false) },
            { "UTF8BOM", Encoding.UTF8 }
        };

        public override object Transform(EngineIntrinsics engineIntrinsics, object inputData)
        {
            switch (inputData)
            {
                case Encoding e:
                    return e;
                case string s:
                    if (StandardEncoding.ContainsKey(s))
                    {
                        return StandardEncoding[s];
                    }
                    else
                    {
                        return Encoding.GetEncoding(s);
                    }
                default:
                    throw new ArgumentTransformationMetadataException();
            }
        }

        public override string ToString() 
        {
            return "[EncodingTransformAttribute]";
        }
    }

    class EncodingCompleter : IArgumentCompleter
    {
        public EncodingCompleter() { }
        public IEnumerable<CompletionResult> CompleteArgument(
            string commandName,
            string parameterName,
            string wordToComplete,
            CommandAst commandAst,
            IDictionary fakeBoundParameters)
        {
            List<string> encodingValues = new List<string>(EncodingTransformAttribute.StandardEncoding.Keys);

            if (!string.IsNullOrEmpty(wordToComplete))
            {
                foreach (string encoding in encodingValues)
                {
                    yield return new CompletionResult(
                        completionText: encoding,
                        listItemText: encoding,
                        resultType: CompletionResultType.ParameterValue,
                        toolTip: string.Format("{0} {1}", "[Encoding]", encoding));
                }
            }
            else
            {
                var filteredList = encodingValues.FindAll(x => x.StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase));
                foreach (string result in filteredList)
                {
                    yield return new CompletionResult(
                        completionText: result,
                        listItemText: result,
                        resultType: CompletionResultType.ParameterValue,
                        toolTip: string.Format("{0} {1}", "[Encoding]", result));
                }
            }
        }
    }
}