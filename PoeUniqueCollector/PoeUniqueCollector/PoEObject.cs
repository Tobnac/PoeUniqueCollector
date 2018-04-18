using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeUniqueCollector
{
    // To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
    //
    //    using QuickType;
    //
    //    var poEObject = PoEObject.FromJson(jsonString);

    namespace QuickType
    {
        using System;
        using System.Net;
        using System.Collections.Generic;

        using Newtonsoft.Json;

        public partial class PoEObject
        {
            [JsonProperty("next_change_id")]
            public string NextChangeId { get; set; }

            [JsonProperty("stashes")]
            public Stash[] Stashes { get; set; }
        }

        public partial class Stash
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("public")]
            public bool Public { get; set; }

            [JsonProperty("accountName")]
            public string AccountName { get; set; }

            [JsonProperty("lastCharacterName")]
            public string LastCharacterName { get; set; }

            [JsonProperty("stash")]
            public string StashStash { get; set; }

            [JsonProperty("stashType")]
            public string StashType { get; set; }

            [JsonProperty("items")]
            public Item[] Items { get; set; }
        }

        public partial class Item
        {
            [JsonProperty("verified")]
            public bool Verified { get; set; }

            [JsonProperty("w")]
            public long W { get; set; }

            [JsonProperty("h")]
            public long H { get; set; }

            [JsonProperty("ilvl")]
            public long Ilvl { get; set; }

            [JsonProperty("icon")]
            public string Icon { get; set; }

            [JsonProperty("league")]
            public string League { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("sockets")]
            public Socket[] Sockets { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("typeLine")]
            public string TypeLine { get; set; }

            [JsonProperty("identified")]
            public bool Identified { get; set; }

            [JsonProperty("note")]
            public string Note { get; set; }

            [JsonProperty("properties")]
            public AdditionalProperty[] Properties { get; set; }

            [JsonProperty("requirements")]
            public AdditionalProperty[] Requirements { get; set; }

            [JsonProperty("explicitMods")]
            public string[] ExplicitMods { get; set; }

            [JsonProperty("flavourText")]
            public string[] FlavourText { get; set; }

            [JsonProperty("frameType")]
            public long FrameType { get; set; }

            [JsonProperty("category")]
            public ItemCategory Category { get; set; }

            [JsonProperty("x")]
            public long X { get; set; }

            [JsonProperty("y")]
            public long Y { get; set; }

            [JsonProperty("inventoryId")]
            public string InventoryId { get; set; }

            [JsonProperty("socketedItems")]
            public SocketedItem[] SocketedItems { get; set; }

            [JsonProperty("support")]
            public bool? Support { get; set; }

            [JsonProperty("additionalProperties")]
            public AdditionalProperty[] AdditionalProperties { get; set; }

            [JsonProperty("nextLevelRequirements")]
            public AdditionalProperty[] NextLevelRequirements { get; set; }

            [JsonProperty("secDescrText")]
            public string SecDescrText { get; set; }

            [JsonProperty("descrText")]
            public string DescrText { get; set; }

            [JsonProperty("implicitMods")]
            public string[] ImplicitMods { get; set; }

            [JsonProperty("utilityMods")]
            public string[] UtilityMods { get; set; }

            [JsonProperty("craftedMods")]
            public string[] CraftedMods { get; set; }

            [JsonProperty("corrupted")]
            public bool? Corrupted { get; set; }

            [JsonProperty("talismanTier")]
            public long? TalismanTier { get; set; }

            [JsonProperty("stackSize")]
            public long? StackSize { get; set; }

            [JsonProperty("maxStackSize")]
            public long? MaxStackSize { get; set; }

            [JsonProperty("duplicated")]
            public bool? Duplicated { get; set; }

            [JsonProperty("artFilename")]
            public string ArtFilename { get; set; }
        }

        public partial class SimpleItem
        { 
            public string Id { get; set; }
            public string TypeLine { get; set; }
            public string PriceCurrency { get; set; }
            public double PriceAmount { get; set; }
        }

        public partial class AdditionalProperty
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("values")]
            public Value[][] Values { get; set; }

            [JsonProperty("displayMode")]
            public long DisplayMode { get; set; }

            [JsonProperty("progress")]
            public double? Progress { get; set; }

            [JsonProperty("type")]
            public long? Type { get; set; }
        }

        public partial class ItemCategory
        {
            [JsonProperty("weapons")]
            public string[] Weapons { get; set; }

            [JsonProperty("gems")]
            public object[] Gems { get; set; }

            [JsonProperty("jewels")]
            public object[] Jewels { get; set; }

            [JsonProperty("accessories")]
            public string[] Accessories { get; set; }

            [JsonProperty("flasks")]
            public object[] Flasks { get; set; }

            [JsonProperty("armour")]
            public string[] Armour { get; set; }

            [JsonProperty("maps")]
            public object[] Maps { get; set; }

            [JsonProperty("currency")]
            public object[] Currency { get; set; }

            [JsonProperty("cards")]
            public object[] Cards { get; set; }
        }

        public partial class SocketedItem
        {
            [JsonProperty("verified")]
            public bool Verified { get; set; }

            [JsonProperty("w")]
            public long W { get; set; }

            [JsonProperty("h")]
            public long H { get; set; }

            [JsonProperty("ilvl")]
            public long Ilvl { get; set; }

            [JsonProperty("icon")]
            public string Icon { get; set; }

            [JsonProperty("support")]
            public bool Support { get; set; }

            [JsonProperty("league")]
            public string League { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("typeLine")]
            public string TypeLine { get; set; }

            [JsonProperty("identified")]
            public bool Identified { get; set; }

            [JsonProperty("properties")]
            public AdditionalProperty[] Properties { get; set; }

            [JsonProperty("additionalProperties")]
            public AdditionalProperty[] AdditionalProperties { get; set; }

            [JsonProperty("requirements")]
            public AdditionalProperty[] Requirements { get; set; }

            [JsonProperty("secDescrText")]
            public string SecDescrText { get; set; }

            [JsonProperty("explicitMods")]
            public string[] ExplicitMods { get; set; }

            [JsonProperty("descrText")]
            public string DescrText { get; set; }

            [JsonProperty("frameType")]
            public long FrameType { get; set; }

            [JsonProperty("category")]
            public SocketedItemCategory Category { get; set; }

            [JsonProperty("socket")]
            public long Socket { get; set; }

            [JsonProperty("colour")]
            public string Colour { get; set; }
        }

        public partial class SocketedItemCategory
        {
            [JsonProperty("gems")]
            public object[] Gems { get; set; }
        }

        public partial class Socket
        {
            [JsonProperty("group")]
            public long Group { get; set; }

            [JsonProperty("attr")]
            public string Attr { get; set; }

            [JsonProperty("sColour")]
            public string SColour { get; set; }
        }

        public partial struct Value
        {
            public long? Integer;
            public string String;
        }

        public partial class PoEObject
        {
            public static PoEObject FromJson(string json) => JsonConvert.DeserializeObject<PoEObject>(json, QuickType.Converter.Settings);
        }

        public partial struct Value
        {
            public Value(JsonReader reader, JsonSerializer serializer)
            {
                Integer = null;
                String = null;

                switch (reader.TokenType)
                {
                    case JsonToken.Integer:
                        Integer = serializer.Deserialize<long>(reader);
                        return;
                    case JsonToken.String:
                    case JsonToken.Date:
                        String = serializer.Deserialize<string>(reader);
                        return;
                }
                throw new Exception("Cannot convert Value");
            }

            public void WriteJson(JsonWriter writer, JsonSerializer serializer)
            {
                if (Integer != null)
                {
                    serializer.Serialize(writer, Integer);
                    return;
                }
                if (String != null)
                {
                    serializer.Serialize(writer, String);
                    return;
                }
                throw new Exception("Union must not be null");
            }
        }

        public static class Serialize
        {
            public static string ToJson(this PoEObject self) => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
        }

        public class Converter : JsonConverter
        {
            public override bool CanConvert(Type t) => t == typeof(Value);

            public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
            {
                if (t == typeof(Value))
                    return new Value(reader, serializer);
                throw new Exception("Unknown type");
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var t = value.GetType();
                if (t == typeof(Value))
                {
                    ((Value)value).WriteJson(writer, serializer);
                    return;
                }
                throw new Exception("Unknown type");
            }

            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters = { new Converter() },
            };
        }
    }

}
