import { TestBed } from '@angular/core/testing';

import { PageIdentService } from './page-ident.service';

describe('PageIdentService', () => {
  let service: PageIdentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PageIdentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
