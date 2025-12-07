import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { StudentService } from '../student.service';

@Component({
  selector: 'app-admission',
  templateUrl: './admission.component.html',
  styleUrls: ['./admission.component.scss']
})
export class AdmissionComponent implements OnInit {
  form: FormGroup;
  classes: any[] = [];
  sections: any[] = [];
  uploadFile?: File;
  message = '';

  constructor(private fb: FormBuilder, private svc: StudentService) {
    this.form = this.fb.group({
      admissionNo: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: [''],
      dateOfBirth: [''],
      classId: [null, Validators.required],
      sectionId: [null, Validators.required],
      guardianName: ['']
    });
  }

  ngOnInit(): void {
    this.svc.getClasses().subscribe(c => this.classes = c);
  }

  onClassChange() {
    const classId = this.form.get('classId')?.value;
    if (classId) {
      this.svc.getSections(classId).subscribe(s => this.sections = s);
    } else {
      this.sections = [];
    }
  }

  onFileSelected(e: any) {
    const f = e.target.files[0];
    if (f) this.uploadFile = f;
  }

  submit() {
    if (this.form.invalid) return;
    const model = { ...this.form.value, academicYear: new Date().getFullYear() };
    this.svc.admit(model).subscribe({
      next: (res) => {
        this.message = 'Admitted successfully. Uploading document (if provided)...';
        if (this.uploadFile) {
          this.svc.uploadDocument(res.id, this.uploadFile).subscribe(() => {
            this.message = 'Admission and document upload successful';
          }, err => this.message = 'Admission saved but document upload failed');
        } else {
          this.message = 'Admission saved successfully';
        }
        this.form.reset();
      },
      error: (err) => this.message = 'Failed to admit: ' + (err?.error?.message || err.statusText)
    });
  }
}
