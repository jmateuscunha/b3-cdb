import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { ResultCdb } from '../models/ResultCdb';

@Injectable({
  providedIn: 'root'
})

export class CdbService {
  private http = inject(HttpClient);
  private resultsSubject = new BehaviorSubject<ResultCdb | null>(null);
  results$ = this.resultsSubject.asObservable();

  private apiUrl = 'http://localhost:5050/api/cdb/calculate';

  calculate(value: number, months: number) {
    this.http.post<ResultCdb>(this.apiUrl, { value, months })
      .subscribe(result => this.resultsSubject.next(result));
  }
}