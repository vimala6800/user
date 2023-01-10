export interface AddPartner {
  partnerID: string;
  partnerName: string;
  location: string;
  countryID: string;
  registeredDate: Date;
  spocUserID: string;
  
  partnerStatus: number;
 
  address1: string;
  billingAddress1: string;
  partnerImage: string;
  skillID: string;
  
  //createdBy: string;
  //created: string;
  //lastModifiedBy: string;
  //lastModified: string
}
export interface AddSPOC {
  id: string,
  userName: string,
  email:string
}
