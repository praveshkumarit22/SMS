import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  base = '/api/students';

  constructor(private http: HttpClient) { }

  getClasses(): Observable<any> {
    return this.http.get(`${this.base}/classes`);
  }

  getSections(classId: any): Observable<any> {
    return this.http.get(`${this.base}/sections/${classId}`);
  }

  validateAdmissionNo(adm: string) {
    return this.http.get(`${this.base}/validate-admissionno?admissionNo=${encodeURIComponent(adm)}`);
  }

  admit(model: any) {
    return this.http.post(`${this.base}/admit`, model);
  }

  uploadDocument(studentId: string, file: File) {
    const fd = new FormData();
    fd.append('file', file, file.name);
    return this.http.post(`${this.base}/${studentId}/documents`, fd);
  }
}
