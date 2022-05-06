import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';


import { RegistrationComponent } from '../registration/registration.component';
import { SignInComponent } from '../sign-in/sign-in.component';

import { ACCES_TOKEN_KEY, AuthorizationServiceService} from '../authorization-module/authorization-service.service';
import { AUTH_API_URL, STORE_API_URL } from '../../models/app-injection-token';
import { environment } from 'src/environments/environment';
import { JwtModule } from '@auth0/angular-jwt';

export function tokenGetter(){
  return localStorage.getItem(ACCES_TOKEN_KEY);
}

@NgModule({
  declarations: [
    RegistrationComponent,
    SignInComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,

    JwtModule.forRoot({
      config: {
        tokenGetter,
        allowedDomains: environment.whiteListedDomains
      }
    })
  ],
  exports: [
    RegistrationComponent,
    SignInComponent,
  ],
  providers: [AuthorizationServiceService,
    {
      provide: AUTH_API_URL,
      useValue: environment.authApi
    },
    {
      provide: STORE_API_URL,
      useValue: environment.storeApi
    }
    
  ]
})
export class AuthorizationModuleModule { }
