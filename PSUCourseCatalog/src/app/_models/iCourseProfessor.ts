export interface ICourseProfessor {
    id: number;
    courseName: string;
    professorName: string;
    professorEmail: string;
    roomNumber?: number;
    sunday: boolean;
    monday: boolean;
    tuesday: boolean;
    wednesday: boolean;
    thursday: boolean;
    friday: boolean;
    saturday: boolean;
}
