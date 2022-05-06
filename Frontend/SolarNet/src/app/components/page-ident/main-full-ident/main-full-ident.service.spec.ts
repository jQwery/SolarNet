import { TestBed } from '@angular/core/testing';

import { MainFullIdentService } from './main-full-ident.service';

describe('MainFullIdentService', () => {
  let service: MainFullIdentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MainFullIdentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
