import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
  ) { }

  getMessage(id: string): Observable<MessageDto> {
    return this.http.get<MessageDto>(this.baseUrl + 'api/message/' + id);
  }

  createMessage(key: string, expiration: Date, content: string): Observable<string> {
    return this.http.post<string>(this.baseUrl + 'api/message', {
      key: key,
      content: content,
      expiresAt: expiration.toISOString()
    });
  }

  decryptMessage(id: string, key: string): Observable<string> {
    return this.http.post<string>(this.baseUrl + 'api/message/' + id, {
      key: key
    });
  }
}

export class MessageDto {
  expiresAt: string;
}
