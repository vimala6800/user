
export interface ProjectManagerAdd {
  projectManagerID: string,
  projectManagerName: string,
  employeeID: string,
  joiningDate: Date,
  pmEmailID: string,
  pmPhoneNumber: string,
  pmPhoto: string,
  pmStatus: number,
  pmUserID: string,
  skillID: string,



}
export interface UserAdd {
  id: string,
  userName: string,
  email: string
}
export interface ProjectManagerSkillView {
  skillID: string,
  skillName: string
}
