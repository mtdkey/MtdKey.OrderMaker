
type FormDataModel = {
    FormModel: FormModel;
    PartModels: PartModel[];
    FieldModels: FieldModel[];
    FormInfoModels: FormInfoModel[];
}

type FormInfoModel = {
    Id: string;
    Name: string;
}

type FormModel = {
    Id: string;
    Name: string;
    Description: string;
    Active: boolean;
    MtdCategory: string;
    Sequence: number;
    Parent: string;
    VisibleNumber: boolean;
    VisibleDate: boolean;
    ImageBack: string;
    ImageBackType: string;
    ImageBackSize: number;
    ImageLogo: string;
    ImageLogoType: string;
    ImageLogoSize: number;
}

type PartModel = {
    Id: string;
    Name: string;
    Description: string;
    FormId: string;
    Sequence: number;
    StyleType: number;
    Title: boolean;
    Active: boolean;
    ImageData: string;
    ImageSize: number;
    ImageType: string;
}

type FieldModel = {
    Id: string;
    Name: string;
    Description: string;
    PartId: string;
    Readonly: boolean;
    Required: boolean;
    Sequence: number;
    Active: boolean;
    SysType: number;
    TriggerId: string;
    DefaultValue: string;
    ListFormId: string;
}



