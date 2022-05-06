import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainIdentComponent } from './main-ident.component';

describe('MainIdentComponent', () => {
  let component: MainIdentComponent;
  let fixture: ComponentFixture<MainIdentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MainIdentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MainIdentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
