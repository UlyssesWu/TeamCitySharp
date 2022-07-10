using System;

namespace TeamCitySharp.Fields
{
  public class BuildSnapshotDependenciesField : IField
  {
    #region Properties

    public BuildField BuildField { get; private set; }
    public bool Count { get; private set; }

    #endregion

    #region Public Methods

    public static BuildSnapshotDependenciesField WithFields(BuildField buildField = null,
                                                         bool count = true)
    {
      return new BuildSnapshotDependenciesField
        {
          BuildField = buildField,
          Count = count,
        };
    }

    #endregion

    #region Overrides IField

    public string FieldId
    {
      get { return "snapshot-dependencies"; }
    }

    public override string ToString()
    {
      var currentFields = string.Empty;

      FieldHelper.AddField(Count, ref currentFields, "count");

      FieldHelper.AddFieldGroup(BuildField, ref currentFields);

      return currentFields;
    }

    #endregion
  }
}