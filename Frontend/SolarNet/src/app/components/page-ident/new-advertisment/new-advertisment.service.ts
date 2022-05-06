import { HttpClient, HttpEvent, HttpParams, HttpRequest } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AUTH_API_URL, STORE_API_URL } from '../../models/app-injection-token';
import { Adv } from '../main-ident/advertisment-content-ident/models/Advertisment';

@Injectable({
  providedIn: 'root'
})
export class NewAdvertismentService {

  constructor(private http: HttpClient, 
    @Inject(AUTH_API_URL) private apiUrl: string,
    @Inject(STORE_API_URL) private storeUrl: string,) { }

  // createAdvertisment(advData: any): 

  imgList: string[] = [];

  postData(advData: any) {
    const body = { price: advData.price, advertismentTitle: advData.advertismentTitle, description: advData.description, images: advData.photoLinks, categoryId: advData.categoryId, city: advData.city }
    return this.http.post(`${this.storeUrl}api/Advertisment/create`, body)
  }

  getAdvValue(id: number): Observable<Adv>{
    return this.http.get<Adv>(`${this.storeUrl}api/Advertisment/${id.toString()}`)
  }
  
  changeAdvValue(advData: any){
    console.log(advData);
    return this.http.post(`${this.storeUrl}api/Advertisment/update`, advData)
  }

  count = 0;

  uploadFile(formData: FormData): Observable<string> {
    return this.http.post(`${this.storeUrl}api/Image/upload`, formData, { responseType: 'text' });//Загрузка файла на сервер
  }

  addDataToImgList(index:any, element: any): void{//Добавление картинки в массив картинок
    if(this.count > 8){}else{
      this.imgList[index] = element;
      console.log(this.imgList);
    }
  }

  deleteDataFromImgList(index: any): void{//Удаление из массива картинок
    this.count--;
    this.imgList.splice(index, 1);
    console.log(this.imgList);
  }
  
}
