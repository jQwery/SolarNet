import { TestBed } from '@angular/core/testing';

import { MainIdentDeactivateGuard } from './main-ident-deactivate.guard';

describe('MainIdentDeactivateGuard', () => {
  let guard: MainIdentDeactivateGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(MainIdentDeactivateGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
