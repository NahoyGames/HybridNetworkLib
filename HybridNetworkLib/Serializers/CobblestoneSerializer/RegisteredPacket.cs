using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HybridNetworkLib.Runtime;

namespace HybridNetworkLib.Serializers.CobblestoneSerializer
{
    class RegisteredPacket
    {
        public Type Type { get; }

        private readonly IEnumerable<FieldInfo> _fields;
        private readonly IEnumerable<PropertyInfo> _properties;

        public RegisteredPacket(Type type)
        {
            Type = type;
            
            _fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(field => Attribute.IsDefined(field, typeof(NetSerializableAttribute)));
            _properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance).Where(property => Attribute.IsDefined(property, typeof(NetSerializableAttribute)));
        }
        
        public byte[] Serialize(object obj, ushort id)
        {
            // Size of the buffer
            var size = ByteWriter.SizeOf(id);
            foreach (var field in _fields)
            {
                int s;
                if ((s = ByteWriter.TrySizeOf(field.GetValue(obj))) >= 0)
                {
                    size += s;
                }
                else
                {
                    Logger.Warn("The field \"" + field.Name + "\" is of unsupported type " + field.FieldType.Name + ".");
                    Logger.Warn("The field \"" + field.Name + "\" will be skipped which may result in unexpected behaviors.");
                }
            }
            foreach (var property in _properties)
            {
                int s;
                if ((s = ByteWriter.TrySizeOf(property.GetValue(obj))) >= 0)
                {
                    size += s;
                }
                else
                {
                    Logger.Warn("The property \"" + property.Name + "\" is of unsupported type " + property.PropertyType.Name + ".");
                    Logger.Warn("The property \"" + property.Name + "\" will be skipped which may result in unexpected behaviors.");
                }
            }
            
            // Fill buffer
            var writer = new ByteWriter(size);
            
            writer.Write(id); // Packet type ID
            foreach (var field in _fields)
            {
                writer.TryWrite(field.GetValue(obj));
            }
            foreach (var property in _properties)
            {
                writer.TryWrite(property.GetValue(obj));
            }

            return writer.Data;
        }

        public object Deserialize(ByteReader reader) // Reader will already have read the packet ID
        {
            object obj = Activator.CreateInstance(Type);

            foreach (var field in _fields)
            {
                field.SetValue(obj, reader.TryRead(field.FieldType));
            }

            foreach (var property in _properties)
            {
                property.SetValue(obj, reader.TryRead(property.PropertyType));
            }

            return obj;
        }
    }

    static class RegisteredPacketExtensions
    {
        public static int IndexOf(this List<RegisteredPacket> list, Type type)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Type == type)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}