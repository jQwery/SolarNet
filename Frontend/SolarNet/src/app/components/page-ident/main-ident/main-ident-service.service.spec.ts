import { TestBed } from '@angular/core/testing';

import { MainIdentServiceService } from './main-ident-service.service';

describe('MainIdentServiceService', () => {
  let service: MainIdentServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MainIdentServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
