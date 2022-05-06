import { TestBed } from '@angular/core/testing';

import { NewAdvertismentService } from './new-advertisment.service';

describe('NewAdvertismentService', () => {
  let service: NewAdvertismentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NewAdvertismentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
