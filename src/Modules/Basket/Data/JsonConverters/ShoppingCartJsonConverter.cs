using System.Text.Json.Serialization;

namespace EShop.Basket.Data.JsonConverters;

public class ShoppingCartJsonConverter : JsonConverter<ShoppingCart>
{
    public override ShoppingCart? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonDocument = JsonDocument.ParseValue(ref reader);
        var rootElement = jsonDocument.RootElement;

        var id = rootElement.GetProperty("Id").GetGuid();
        var userName = rootElement.GetProperty("UserName").GetString();
        var itemsElement = rootElement.GetProperty("Items");

        var shoppingCart = ShoppingCart.Create(id, userName);

        var items = itemsElement.Deserialize<List<ShoppingCartItem>>(options);
        if (items is not null)
        {
            //TODO: refactor
            shoppingCart.AddItemsFromJson(items);
        }

        return shoppingCart;
    }

    public override void Write(Utf8JsonWriter writer, ShoppingCart value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("Id", value.Id.ToString());
        writer.WriteString("UserName", value.UserName);

        writer.WritePropertyName("Items");
        JsonSerializer.Serialize(writer, value.Items, options);
       
        writer.WriteEndObject();
    }
}
