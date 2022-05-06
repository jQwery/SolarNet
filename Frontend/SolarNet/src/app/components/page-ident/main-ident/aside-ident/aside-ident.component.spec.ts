import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AsideIdentComponent } from './aside-ident.component';

describe('AsideIdentComponent', () => {
  let component: AsideIdentComponent;
  let fixture: ComponentFixture<AsideIdentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AsideIdentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AsideIdentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
