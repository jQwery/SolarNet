import { TestBed } from '@angular/core/testing';

import { MainIdentGuard } from './main-ident.guard';

describe('MainIdentGuard', () => {
  let guard: MainIdentGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(MainIdentGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
