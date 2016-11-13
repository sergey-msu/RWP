using System;
using System.Collections.Generic;
using System.Linq;
using RWP.App.Infrastructure.Mvvm;
using RWP.Data;
using RWP.App.Infrastructure;

namespace RWP.App.Models
{
  public class ReportSettingsModel : ModelBase<ReportSetting>
  {
    private const string PARAM_SEPARATOR = ";";
    private const string PARAM_PAIR_SEPARATOR = "=";
    private const string PARAM_PAIR_FORMAT = "{0}={1}";

    private Dictionary<string, object> _settings;

    public ReportSettingsModel(ReportSetting entity)
      : base(entity)
    {
      Validate();
    }

    #region Properties

    private string _name;
    public string Name
    {
      get { return _name; }
      set
      {
        _name = value;
        Validate("Name");
        OnPropertyChanged("Name");
      }
    }

    private string _type;
    public string Type
    {
      get { return _type; }
      set
      {
        _type = value;
        Validate("Type");
        OnPropertyChanged("Type");
      }
    }

    private Dictionary<string, object> Settings
    {
      get
      {
        if (_settings == null)
          _settings = new Dictionary<string, object>();

        return _settings;
      }
    }

    public T GetSetting<T>(string settingName)
    {
      object result;
      if (!Settings.TryGetValue(settingName, out result))
        Constants.DFT_SETTINGS.TryGetValue(settingName, out result);

      return (T)result;
    }

    public void SetSetting(string settingName, object value)
    {
      Settings[settingName] = value;
    }

    public bool IsDefault()
    {
      return Settings.ContainsKey(Constants.FONT_FAMILY_SETTING) && (string)Settings[Constants.FONT_FAMILY_SETTING] == Constants.DFT_FONT &&
            Settings.ContainsKey(Constants.FONT_SIZE_SETTING) && (int)Settings[Constants.FONT_SIZE_SETTING] == Constants.DFT_FONT_SIZE &&
            Settings.ContainsKey(Constants.LINE_MARGIN_SETTING) && (int)Settings[Constants.LINE_MARGIN_SETTING] == Constants.DFT_LINE_MARGIN &&
            Settings.ContainsKey(Constants.DOC_MARGIN_SETTING) && (int)Settings[Constants.DOC_MARGIN_SETTING] == Constants.DFT_DOC_MARGIN;
    }

    #endregion Properties

    protected override void ConfigureValidation()
    {
      Validator.AddRule("Name", () => !string.IsNullOrWhiteSpace(Name), FIELD_ISREQUIRED_ERROR);
      Validator.AddRule("Type", () => !string.IsNullOrWhiteSpace(Type), FIELD_ISREQUIRED_ERROR);
    }

    protected override void ResetFromSourceImpl()
    {
      this.Name = Entity.Name;
      this.Type = Entity.Type;

      var strSettings = new Dictionary<string, string>();
      if (!string.IsNullOrWhiteSpace(Entity.Settings))
      {
        var settPairs = Entity.Settings.Split(new[] { PARAM_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var pair in settPairs)
        {
          var kv = pair.Split(new[] { PARAM_PAIR_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
          if (kv.Length != 2) continue;
          strSettings[kv[0]] = kv[1];
        }
      }
      else
      {
        Settings.Clear();
      }

      string font;
      if (!strSettings.TryGetValue(Constants.FONT_FAMILY_SETTING, out font))
        font = Constants.DFT_FONT;
      SetSetting(Constants.FONT_FAMILY_SETTING, font);

      string strSize;
      int size;
      if (!strSettings.TryGetValue(Constants.FONT_SIZE_SETTING, out strSize) || !int.TryParse(strSize, out size))
        size = Constants.DFT_FONT_SIZE;
      SetSetting(Constants.FONT_SIZE_SETTING, size);


      string strLineMargin;
      int lineMargin;
      if (!strSettings.TryGetValue(Constants.LINE_MARGIN_SETTING, out strLineMargin) || !int.TryParse(strLineMargin, out lineMargin))
        lineMargin = Constants.DFT_LINE_MARGIN;
      SetSetting(Constants.LINE_MARGIN_SETTING, lineMargin);


      string strDocMargin;
      int docMargin;
      if (!strSettings.TryGetValue(Constants.DOC_MARGIN_SETTING, out strDocMargin) || !int.TryParse(strDocMargin, out docMargin))
        docMargin = Constants.DFT_DOC_MARGIN;
      SetSetting(Constants.DOC_MARGIN_SETTING, docMargin);
    }

    protected override void SaveToSourceImpl()
    {
      Entity.Name = this.Name;
      Entity.Type = this.Type;

      if (Settings.Any())
      {
        Entity.Settings = string.Join(PARAM_SEPARATOR, Settings.Select(s => string.Format(PARAM_PAIR_FORMAT, s.Key, s.Value.ToString())));
      }
      else
      {
        Entity.Settings = null;
      }
    }
  }
}
