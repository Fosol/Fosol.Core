﻿using Fosol.Core.Xml.Attributes;
using System;

namespace Fosol.Core.Xml
{
    /// <summary>
    /// XmlSchemaDataType enumerator is a list of XML schema datatypes.
    /// </summary>
    /// <see cref="http://www.w3.org/2001/XMLSchema-datatypes"/>
    public enum XmlSchemaDataType
    {
        [XmlSchemaDataType("string")]
        String,
        [XmlSchemaDataType("boolean")]
        Boolean,
        [XmlSchemaDataType("float")]
        Float,
        [XmlSchemaDataType("double")]
        Double,
        [XmlSchemaDataType("decimal")]
        Decimal,
        [XmlSchemaDataType("dateTime")]
        DateTime,
        [XmlSchemaDataType("duration")]
        Duration,
        [XmlSchemaDataType("hexBinary")]
        HexBinary,
        [XmlSchemaDataType("base64Binary")]
        Base64Binary,
        [XmlSchemaDataType("anyURI")]
        AnyURI,
        [XmlSchemaDataType("ID")]
        Id,
        [XmlSchemaDataType("IDREF")]
        IdRef,
        [XmlSchemaDataType("ENTITY")]
        Entity,
        [XmlSchemaDataType("NOTATION")]
        Notation,
        [XmlSchemaDataType("normalizedString")]
        NormalizedString,
        [XmlSchemaDataType("token")]
        Token,
        [XmlSchemaDataType("language")]
        Language,
        [XmlSchemaDataType("IDREFS")]
        IdRefs,
        [XmlSchemaDataType("ENTITIES")]
        Entities,
        [XmlSchemaDataType("NMTOKEN")]
        NmToken,
        [XmlSchemaDataType("NMTOKENS")]
        NmTokens,
        [XmlSchemaDataType("Name")]
        Name,
        [XmlSchemaDataType("QName")]
        QName,
        [XmlSchemaDataType("NCName")]
        NcName,
        [XmlSchemaDataType("integer")]
        Integer,
        [XmlSchemaDataType("nonNegativeInteger")]
        NonNegativeInteger,
        [XmlSchemaDataType("positiveInteger")]
        PositiveInteger,
        [XmlSchemaDataType("nonPositiveInteger")]
        NonPositiveInteger,
        [XmlSchemaDataType("negativeInteger")]
        NegativeInteger,
        [XmlSchemaDataType("byte")]
        Byte,
        [XmlSchemaDataType("int")]
        Int,
        [XmlSchemaDataType("long")]
        Long,
        [XmlSchemaDataType("short")]
        Short,
        [XmlSchemaDataType("unsignedByte")]
        UnsignedByte,
        [XmlSchemaDataType("unsignedInt")]
        UnsignedInt,
        [XmlSchemaDataType("unsignedLong")]
        UnsignedLong,
        [XmlSchemaDataType("unsignedShort")]
        UnsignedShort,
        [XmlSchemaDataType("date")]
        Date,
        [XmlSchemaDataType("time")]
        Time,
        [XmlSchemaDataType("gYearMonth")]
        GYearMonth,
        [XmlSchemaDataType("gYear")]
        GYear,
        [XmlSchemaDataType("gMonthDay")]
        GMonthDay,
        [XmlSchemaDataType("gDay")]
        GDay,
        [XmlSchemaDataType("gMonth")]
        GMonth
    }
}
