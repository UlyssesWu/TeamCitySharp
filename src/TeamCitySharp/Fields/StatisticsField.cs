﻿using System;

namespace TeamCitySharp.Fields
{
  public class StatisticsField : IField
  {
    #region Properties

    public PropertyField PropertyField { get; private set; }
    public bool Count { get; private set; }
    public bool Href { get; private set; }

    #endregion

    #region Public Methods

    public static StatisticsField WithFields(PropertyField propertyField = null,
                                             bool count = true,
                                             bool href= false)
    {
      return new StatisticsField
        {
          PropertyField = propertyField,
          Count = count,
          Href = href
        };
    }

    #endregion

    #region Overrides IField

    public string FieldId
    {
      get { return "statistics"; }
    }

    public override string ToString()
    {
      var currentFields = string.Empty;

      FieldHelper.AddField(Count, ref currentFields, "count");
      FieldHelper.AddField(Href, ref currentFields, "href");

      FieldHelper.AddFieldGroup(PropertyField, ref currentFields);

      return currentFields;
    }

    #endregion
  }
}