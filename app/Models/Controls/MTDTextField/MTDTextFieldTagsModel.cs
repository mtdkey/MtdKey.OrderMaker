using Microsoft.AspNetCore.Razor.Language.Intermediate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MtdKey.OrderMaker.Models.Controls.MTDTextField
{
    public class MTDTextFieldTagsModel : MTDTextFieldTags
    {
        /*Required or Disabled Attribute*/
        public string RDAttribute { get; set; }
        public string IdLabel { get; set; }
        public string IdHelper { get; set; }
        public string IdHelperError { get; set; }
        public string IdInput { get; set; }

        public MTDTextFieldTagsModel(MTDTextFieldTags tags) {
            this.Id = tags.Id ?? Guid.NewGuid().ToString(); 
            this.MaxLength = tags.MaxLength;
            this.Disabled = tags.Disabled;
            this.HelperText = tags.HelperText;
            this.HelperTextLocalizer = tags.HelperTextLocalizer;
            this.HelperError = tags.HelperError;
            this.HelperErrorLocalizer = tags.HelperErrorLocalizer;
            this.IconLeading = tags.IconLeading;
            this.IconLeadingScript = tags.IconLeadingScript;     
            this.IconTrailing = tags.IconTrailing;
            this.IconTrailingString = tags.IconTrailingString;
            this.IconTrailingColor = tags.IconTrailingColor;
            this.Label = tags.Label;
            this.LabelLocalized = tags.LabelLocalized;
            this.Name = tags.Name;
            this.Placeholder = tags.Placeholder;
            this.PlaceholderLocalized = tags.PlaceholderLocalized;
            this.Required = tags.Required;
            this.Type = tags.Type ?? "text";
            this.Value = tags.Value;
            this.Class = tags.Class ?? string.Empty;
            this.Step = tags.Step ?? string.Empty;

            this.IdLabel = $"{Id}-label";
            this.IdHelper = $"{Id}-helper";
            this.IdHelperError = $"{Id}-helper-error";
            this.IdInput = $"{Id}-input";            
            this.RDAttribute = tags.Required ? "required" : string.Empty;
            if (tags.Disabled) { RDAttribute = "disabled"; }

            this.ShowCounter = tags.ShowCounter;
            this.MaxLength = tags.MaxLength == 0 ? 250 : tags.MaxLength;
            this.UnlimitedText = tags.UnlimitedText;

            this.MTDTexFieldView = tags.MTDTexFieldView;
            this.Cols = tags.Cols == 0 ? 40 : tags.Cols;
            this.Rows = tags.Rows == 0 ? 8 : tags.Rows;

            this.MtdInputClicker = tags.MtdInputClicker;
            this.Pattern = tags.Pattern;
            this.MtdDataMessage = tags.MtdDataMessage;
                
                       
        }
                
    }
}
