/*
The MIT License(MIT)

Copyright(c) 2017 AT&T

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
fu
The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.
rnished to do so, subject to the following conditions:

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dali.Data
{
    public static class Globals
    {
        public static bool tileIdForFirstViewSet = false;
        public static List<Mark> marks;
        public static Mark selectedMark;
        public static string tileId;
        public static Dictionary<string, Mark> selectedMarksOnView = new Dictionary<string, Mark>();
        public static string serverUrl;
    }

    public class Mark
    {
        [JsonProperty(PropertyName = "Id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "Label")]
        public string label { get; set; }
        [JsonProperty(PropertyName = "Type")]
        public string type { get; set; }
    }

    public class MarkStringContent : Mark
    {
        [JsonProperty(PropertyName = "Content")]
        public string content { get; set; }
    }

    public class MarkListContent : Mark
    {
        [JsonProperty(PropertyName = "Content")]
        public List<string> content { get; set; }
    }

    public class NewId
    {
        [JsonProperty(PropertyName = "code")]
        public int code { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string newid { get; set; }
    }

}
