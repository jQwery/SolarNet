import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { AUTH_API_URL, STORE_API_URL } from '../../models/app-injection-token';
import { Adv } from './advertisment-content-ident/models/Advertisment';
import { User } from './user/user-model/user-model';

@Injectable({
  providedIn: 'root'
})
export class MainIdentServiceService {

  private allCategoryParams = {//Обьект с параметрами для запроса обьявлений
    limit: 10,//максимум обьявлений на однк страницу
    offset: 0,//текущая страница
    //aside
    idCategory: '',//категория
    date: '',//дата
    minimumCost: 0,//цена от
    maximumCost: 0,//цена до
    onlyWithPhoto: false,//с фото
    onlyWithComments: false,//с комментариями
    city: '',//город
    //sorting
    byDescending: true,
    // byDescendingCost: false,//по цене
    sortByCost: false,
    sortByDate: true,
    //searching
    keyWords: '',//строка поиска
    advListType: 'start',
    showHiddenAdvs: false
  }

  otherUserId: string = '';
  otherUserName: string = '';

  private paramsInfo = new BehaviorSubject(this.allCategoryParams);//Обьект для отслеживания изменений переменной, которая в параметрах

  getParamsInfo(): Observable<any>{//возвращает изменения в переменной, которвя передана как аргумент обьекту выше
    return this.paramsInfo.asObservable();
  }

  getParams(): any{//в обьекте так же есть геттер
    return this.paramsInfo.getValue();
  }
  
  setParamsInfo(paramsList: any): void{//и сеттер
    this.paramsInfo.next(paramsList);
  }

  clearAllCategoryParams(): void{//возвращает все параметры в начальное состояние
    this.allCategoryParams.offset = 0;
    this.allCategoryParams.idCategory = '';
    this.allCategoryParams.date = '';
    this.allCategoryParams.minimumCost = 0;
    this.allCategoryParams.maximumCost = 0;
    this.getMaxCostS();
    this.allCategoryParams.city = '';
    this.allCategoryParams.byDescending = true;
    this.allCategoryParams.sortByCost = false;
    this.allCategoryParams.sortByDate = true;
    this.allCategoryParams.keyWords = '';
    this.allCategoryParams.advListType = 'start';
    this.allCategoryParams.showHiddenAdvs = false;
    this.setParamsInfo(this.allCategoryParams);
  }

  setAllCategoryParamsAside(newParams: any): void{//Установка параметров обьявлений
    // this.allCategoryParams.keyWords = newParams.keyWords;
    this.allCategoryParams.offset = 0;
    this.allCategoryParams.idCategory = newParams.idCategory;
    this.allCategoryParams.date = newParams.date;
    this.allCategoryParams.minimumCost= newParams.minimumCost;
    this.allCategoryParams.maximumCost = newParams.maximumCost;
    this.allCategoryParams.onlyWithPhoto = newParams.onlyWithPhoto;
    this.allCategoryParams.onlyWithComments = newParams.onlyWithComments;
    this.allCategoryParams.city = newParams.city;
    this.setParamsInfo(this.allCategoryParams);
  }

  setAllCategoryParamsSort(newParams: any): void{//Установка сортировки обьявлений
    this.allCategoryParams.offset = 0;
    this.allCategoryParams.byDescending = this.changeParamToBool(newParams.sortType);
    if(newParams.sortParamType == 'По цене'){
      this.allCategoryParams.sortByCost = true;
      this.allCategoryParams.sortByDate = false;
    }else{
      this.allCategoryParams.sortByDate = true;
      this.allCategoryParams.sortByCost = false;
    }
    this.setParamsInfo(this.allCategoryParams);
  }

  changeParamToBool(param: string): boolean{//так как из компонента значения приходят в виде строк, то необходимо их преобразовать в логическое значение
    if(param == 'По возрастанию'){//если возрастание то true
      return false;
    }else{
      return true;//если убывание то false
    }
  }

  changeParamToString(param: boolean): string{//меняет логическую переменную на строковую
    if(param == false){//
      return 'По возрастанию';
    }else{
      return 'По убыванию';//
    }
  }

  setAllCategoryParamsSearch(newParams: any): void{//Установка параметра поиска обьявлений
    this.allCategoryParams.offset = 0;
    this.allCategoryParams.idCategory = newParams.idCategory;
    this.allCategoryParams.keyWords = newParams.keyWords;
    this.allCategoryParams.maximumCost = 0;
    this.getMaxCostS();
    this.setParamsInfo(this.allCategoryParams);
  }

  setAllParamsOffset(newParams: any): void{//Параметры пагинации
    this.allCategoryParams.offset = newParams.offset;
    this.setParamsInfo(this.allCategoryParams);
  }

  setAdvsType(newType: string): void{//смена статуса пользовательских обьявлений
    this.allCategoryParams.advListType = newType;
    this.allCategoryParams.offset = 0;
    this.setParamsInfo(this.allCategoryParams);
  }

  setShowHiddenAdvsStatus(newParams:any): void{//Параметры отображения скрытых обьявлений
    this.allCategoryParams.showHiddenAdvs = newParams.showHiddenAdvs;
    this.setParamsInfo(this.allCategoryParams);
  }
  
  constructor(private http: HttpClient, 
              @Inject(STORE_API_URL) private storeUrl: string){
    this.getMaxCostS();
  }

  getMaxCostS(): void{//Получение максимальной стоимости
        this.getMaxCost().subscribe(data => {
        this.allCategoryParams.maximumCost = data.cost + 1;//Для корректной работы запросов
    });
  }

  getAdvertismentsFromServer(): Observable<Adv[]>{//запрос обьявлений с сервера по всем параметрам
        return this.http.get<Adv[]>(`${this.storeUrl}api/Advertisment/all`, {
              params: new HttpParams()
              .set('Limit', this.allCategoryParams.limit.toString())
              .set('Offset', this.allCategoryParams.offset.toString())
              .set('IdCategory', this.allCategoryParams.idCategory.toString())
              .set('Date', this.allCategoryParams.date.toString())
              .set('MinimumCost', this.allCategoryParams.minimumCost.toString())
              .set('MaximumCost', this.allCategoryParams.maximumCost.toString())
              .set('OnlyWithPhoto', this.allCategoryParams.onlyWithPhoto.toString())
              .set('OnlyWithComments', this.allCategoryParams.onlyWithComments.toString())
              .set('ByDescending', this.allCategoryParams.byDescending.toString())
              .set('KeyWords', this.allCategoryParams.keyWords.toString())
              .set('City', this.allCategoryParams.city.toString())
              .set('SortByCost', this.allCategoryParams.sortByCost.toString())
              .set('SortByDate', this.allCategoryParams.sortByDate.toString())
        });
  }  

  getUserAdvList(): Observable<Adv[]>{//запрос пользовательских обьявлений
    return this.http.get<Adv[]>(`${this.storeUrl}api/Advertisment/my`, {
      params: new HttpParams()
        .set('HideDeleted', this.allCategoryParams.showHiddenAdvs.toString())
        .set('Limit', this.allCategoryParams.limit.toString())
        .set('Offset', this.allCategoryParams.offset.toString())
    })
  }

  getOtherUserAdvList(userId: any): Observable<Adv[]>{//ОБьявления другого пользователя
    return this.http.get<Adv[]>(`${this.storeUrl}api/Advertisment/byUserId`, {
      params: new HttpParams()
        .set('UsedId', userId.toString())
        .set('Limit', this.allCategoryParams.limit.toString())
        .set('Offset', this.allCategoryParams.offset.toString())
    })
  }

  getNotApprovedAdvList(): Observable<Adv[]>{//Запросы на обьявления
    return this.http.get<Adv[]>(`${this.storeUrl}api/Advertisment/new-advertisments`, {
      params: new HttpParams()
        .set('Limit', this.allCategoryParams.limit.toString())
        .set('Offset', this.allCategoryParams.offset.toString())
        .set('IdCategory', this.allCategoryParams.idCategory.toString())
        .set('Date', this.allCategoryParams.date.toString())
        .set('MinimumCost', this.allCategoryParams.minimumCost.toString())
        .set('MaximumCost', this.allCategoryParams.maximumCost.toString())
        .set('OnlyWithPhoto', this.allCategoryParams.onlyWithPhoto.toString())
        .set('OnlyWithComments', this.allCategoryParams.onlyWithComments.toString())
        .set('ByDescending', this.allCategoryParams.byDescending.toString())
        .set('KeyWords', this.allCategoryParams.keyWords.toString())
        .set('City', this.allCategoryParams.city.toString())
        .set('SortByCost', this.allCategoryParams.sortByCost.toString())
        .set('SortByDate', this.allCategoryParams.sortByDate.toString())
    })
  }

  getMaxCost(): Observable<any>{//Получение максимальной цены
      return this.http.get<any>(`${this.storeUrl}api/Advertisment/expensive`, {
            params: new HttpParams()
            .set('IdCategory', this.allCategoryParams.idCategory.toString())
            .set('Date', this.allCategoryParams.date.toString())
            .set('OnlyWithPhoto', this.allCategoryParams.onlyWithPhoto.toString())
            .set('OnlyWithComments', this.allCategoryParams.onlyWithComments.toString())
            .set('SortByCost', this.allCategoryParams.sortByCost.toString())
            .set('City', this.allCategoryParams.city.toString())
      });
  }

  getLastPageNumber(status: any): Observable<any>{//Последняя страница для разных типов обьявлений
      return this.http.get<any>(`${this.storeUrl}api/Advertisment/count`, {
            params: new HttpParams()
            .set('IdCategory', this.allCategoryParams.idCategory.toString())
            .set('AdvertismentsPerPage', this.allCategoryParams.limit.toString())
            .set('Date', this.allCategoryParams.date.toString())            
            .set('MinimumCost', this.allCategoryParams.minimumCost.toString())
            .set('MaximumCost', this.allCategoryParams.maximumCost.toString())
            .set('OnlyWithPhoto', this.allCategoryParams.onlyWithPhoto.toString())
            .set('OnlyWithComments', this.allCategoryParams.onlyWithComments.toString())
            .set('KeyWords', this.allCategoryParams.keyWords.toString())
            .set('City', this.allCategoryParams.city.toString())
            .set('Status', status.toString())
      });
  }

  getLastPageNumberForMyAdvs(): Observable<any>{
    return this.http.get<any>(`${this.storeUrl}api/Advertisment/my/count`, {
      params: new HttpParams()
      .set('HideDeleted', 'false')
      .set('Limit', this.allCategoryParams.limit.toString())
      .set('Offset', this.allCategoryParams.offset.toString())
    })
  }

  getLastPageNumberForUserList(): Observable<any>{
    return this.http.get<any>(`${this.storeUrl}api/User/all/countPages`, {
      params: new HttpParams()
      .set('Limit', this.allCategoryParams.limit.toString())
      .set('Offset', this.allCategoryParams.offset.toString())
    })
  }

  getLastPageNumberForOtherUserAdvs(userId: any): Observable<any>{
    return this.http.get<any>(`${this.storeUrl}api/Advertisment/byUserId/count`, {
      params: new HttpParams()
        .set('UsedId', userId.toString())
        .set('Limit', this.allCategoryParams.limit.toString())
        .set('Offset', this.allCategoryParams.offset.toString())
    })
  }

  // Удаление обьявления
  deleteAdv(advId: any){
    return this.http.delete(`${this.storeUrl}api/Advertisment/delete/${advId.toString()}`)
  }

  approveAdv(advId: any){//Одобрение обьявления
    return this.http.post(`${this.storeUrl}api/Advertisment/approve/${advId.toString()}`, {id: advId.toString()});
  }

  rejectAdv(advId: any){//Отклонение обьявления
    return this.http.post(`${this.storeUrl}api/Advertisment/reject`, {advertismentId: advId.toString(), deleteReason: 'потому что'});
  }

  getAllUsers(): Observable<User[]>{//Получение списка пользователей
    return this.http.get<User[]>(`${this.storeUrl}api/User/all`, {
      params: new HttpParams()
        .set('Limit', this.allCategoryParams.limit.toString())
        .set('Offset', this.allCategoryParams.offset.toString())
    })
  }

  deleteUser(userId: any){//Бан поьзователч
    return this.http.delete(`${this.storeUrl}api/User/id`, {
      params: new HttpParams()
        .set('id', userId.toString())
    });
  }
}
