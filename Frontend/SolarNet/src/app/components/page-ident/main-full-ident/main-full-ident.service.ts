import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { AUTH_API_URL, STORE_API_URL } from '../../models/app-injection-token';
import { Adv } from '../main-ident/advertisment-content-ident/models/Advertisment';

@Injectable({
  providedIn: 'root'
})
export class MainFullIdentService {

  currentAdvValue: any;

  advChanged: any ={
    changed: ''
  }

  private advChangedInfo = new BehaviorSubject(this.advChanged);//Для подписки на изменение данных обьявления

  getadvChangedInfo(): Observable<any>{//возвращает обьект, на изменения которого можно подписаться
    return this.advChangedInfo.asObservable();
  }

  getadvChanged(): any{//в обьекте так же есть геттер
    return this.advChangedInfo.getValue();
  }
  
  setadvChangedInfo(newStatus: any): void{//и сеттер
    this.advChangedInfo.next(newStatus);
  }

  commentParams: any = {
    limit: 10,
    offset: 0
  }
  constructor(private http: HttpClient, 
    @Inject(AUTH_API_URL) private apiUrl: string,
    @Inject(STORE_API_URL) private storeUrl: string) { }

  changeCurrentAdvValue(newValue: any): void{
    this.currentAdvValue = newValue;
    console.log(this.currentAdvValue);
  }

  getAdvValue(id: number): Observable<Adv>{//Получение отдельного обьявления по id
    return this.http.get<Adv>(`${this.storeUrl}api/Advertisment/${id.toString()}`)
  }

  getCommentValue(id: number): Observable<any[]>{//Получение комментаириев к текущему обьялвению
    return this.http.get<any[]>(`${this.storeUrl}api/Comment/comments-of-advertisment`, {
      params: new HttpParams()
        .set("AdvertismentId", id.toString())
        .set("Limit", this.commentParams.limit.toString())
        .set("Offset", this.commentParams.offset.toString())
    })
  }

  addComment(commentData: any) {//Добавить коммент
    const body = { advertismentId: commentData.advertismentId, text: commentData.text}
    return this.http.post(`${this.storeUrl}api/Comment/add-comment`, body)
  }

  deleteComment(commentId: any){//Удалить коммент
    return this.http.delete(`${this.storeUrl}api/Comment/delete/${commentId.toString()}`)
  }
  
}
