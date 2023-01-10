export class User {
  public id: string = "";
  public userName: string = "";
  public email: string = "";
  public phoneNumber: string = "";
  public roles: string[] = [];
 /* public ClientURI: string = "";*/
 
  
 
  constructor(id: string, userName: string, email: string, phoneNumber: string, roles: string[]) {
    this.id = id;
    this.userName = userName;
    this.email = email;
    this.phoneNumber = phoneNumber;
    this.roles = roles;
  /* // this.ClientURI = 'https://localhost:4200/resetpassword';*/
    
    
  }
}
