export interface Partner {
  partnerID: string;
  partnerName: string;
  location: string;
  countryID: string;
  registeredDate: string;
  spocUserID: string;
  address1: string;
  billingAddress1: string;
  partnerImage: string;
  partnerStatus: number;
  SkillID: string;
}

export interface CountryMaster {
  countryID: number;
  countryName: string;
}
export interface SkillMaster {
  skillID: number;
  skillName: string;
}

export interface PartnerModel {
  partnerID: string;
  partnerName: string;
  location: string;
  country: string
  registeredDate: Date;
  partnerStatus: number;
  address1: string;
  billingAddress1: string;
}


