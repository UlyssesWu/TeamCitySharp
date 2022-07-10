using System;

namespace TeamCitySharp.Fields
{
  public class InvestigationsField : IField
  {
    #region Properties
    public bool Count { get; private set; }
    public bool NextHref { get; private set; }
    public bool PrevHref { get; private set; }
    public bool Href { get; private set; }

    #endregion

    #region Public Methods

    public static InvestigationsField WithFields(bool count = true,bool nextHref=false, bool prevHref = false, bool href = false)
    {
      return new InvestigationsField
      {
        Count = count,
        NextHref = nextHref,
        PrevHref = prevHref,
        Href = href
      };
    }

    #endregion

    #region Overrides IField

    public string FieldId
    {
      get { return "investigations"; }
    }

    public override string ToString()
    {
      var currentFields = string.Empty;

      FieldHelper.AddField(Count, ref currentFields, "count");
      FieldHelper.AddField(NextHref, ref currentFields, "nextHref");
      FieldHelper.AddField(PrevHref, ref currentFields, "prevHref");
      FieldHelper.AddField(Href, ref currentFields, "href");

      return currentFields;
    }

    #endregion
  }
}
